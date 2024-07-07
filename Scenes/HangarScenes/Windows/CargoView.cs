using System.Collections.Generic;
using System.Linq;
using GloryOfRitiria.Scenes.Utils;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class CargoView : PanelContainer
{
	public GlobalSignals Signals;
	private List<Cargo> _cargoToAdd;
	private List<Cargo> _addedCargo;
	private List<Cargo> _cargoToStore;
	// When player adds more cargo to ship, cargo is added here. When outfitting is finished, cargo is transferred to the ship 
	private Ship _ship;
	private GridContainer _cargoGrid;

	/** if true then store replace cargo too*/
	private bool _replaceMode;
	private Cargo _replacedCargo;


	public override void _Ready()
	{
		Signals.Connect(nameof(Signals.CargoSelectedForOutfit), new Callable(this, nameof(SaveCargo)));

		// This is very messy but in theory only cargo buttons are present there
		foreach (var node in _cargoGrid.GetChildren())
		{
			if (node is MarginContainer)
			{
				
			}
		}
	}

	public void Init(Ship ship)
	{
		_cargoToAdd = new List<Cargo>(ship.GetCargo());
		_cargoToStore = new List<Cargo>();
		_addedCargo = new List<Cargo>();
		_ship = ship;
		_cargoGrid = GetNode<GridContainer>("GridCont");

		foreach (var cargo in _cargoToAdd)
		{
			_cargoGrid.AddChild(_buildCargoButton(cargo));
		}
		
		if (!ship.IsCargoFull())
			_cargoGrid.AddChild(_buildAddCargoButton());
	}

	/**Updates the visuals to represent the new content*/
	public void Update()
	{
		// Clear view
		foreach (var node in _cargoGrid.GetChildren())
		{
			node.QueueFree();
		}
		
		// Read everything
		foreach (var cargo in _cargoToAdd)
		{
			_cargoGrid.AddChild(_buildCargoButton(cargo));
		}
		
		if (_cargoToAdd.Count < _ship.GetCargoCapacity())
			_cargoGrid.AddChild(_buildAddCargoButton());
	}

	/**Returns the amount of cargo that will be in the ship after outfitting*/
	public int GetCargoCount()
	{
		return _cargoToAdd.Count;
	}
	
	/** Adds, replaces and stores the cargo for real this time */
	public void OutfitCargo(Ship ship)
	{
		ship.SetCargo(_cargoToAdd);
		foreach (var cargo in _cargoToStore)
		{
			game_state.AddCargo(cargo);
		}
	}

	public void ResetGameState()
	{
		foreach (var cargo in _addedCargo)
		{
			game_state.AddCargo(cargo);
		}
	}
	
	private MarginContainer _buildCargoButton(Cargo cargo)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/OutfitCargoButton.tscn");
		var inst = (OutfitCargoButton)scene.Instantiate();
		inst.InitFullButton(cargo, _addedCargo.Contains(cargo));
		
		var button = inst.GetNode<ExtTextureButton>("PanelCont/ExtTextureButton");
		button.LeftPressed += () => ReplaceCargo(cargo);
		button.RightPressed += () => RemoveCargo(cargo);
		
		return inst;
	}
	
	private MarginContainer _buildAddCargoButton()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/OutfitCargoButton.tscn");
		var inst = (OutfitCargoButton)scene.Instantiate();
		inst.InitAddButton();
		
		var button = inst.GetNode<ExtTextureButton>("PanelCont/ExtTextureButton");
		button.LeftPressed += AddButtonPressed;
		return inst;
	}

	private void ReplaceCargo(Cargo cargo)
	{
		Signals.EmitSignal(nameof(Signals.SimpleButtonClicked));
		/*Needs to store cargo, open the add window but also be able to put the stored cargo back on ship if add window closed*/
		_replaceMode = true;
		_replacedCargo = cargo;
		Signals.EmitSignal(nameof(Signals.AddCargoClicked)); // open selection window
	}
	
	private void RemoveCargo(Cargo cargo)
	{
		Signals.EmitSignal(nameof(Signals.SimpleButtonClicked));
		_cargoToAdd.Remove(cargo);
		_cargoToStore.Add(cargo);
		Signals.EmitSignal(nameof(Signals.CargoButtonClicked)); // Updates visuals
	}

	private void AddButtonPressed()
	{
		_replaceMode = false;
		Signals.EmitSignal(nameof(Signals.AddCargoClicked)); // open selection window
		Signals.EmitSignal(nameof(Signals.SimpleButtonClicked));
	}

	public void SaveCargo(string cargoName)
	{
		var cargo = game_state.PopCargo(cargoName);
		_cargoToAdd.Add(cargo);
		_addedCargo.Add(cargo);
		if (_replaceMode)
		{
			_cargoToAdd.Remove(_replacedCargo);
			_cargoToStore.Add(_replacedCargo);
		}
	}
}
