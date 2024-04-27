using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class CargoView : PanelContainer
{
	public GlobalSignals Signals;
	private List<Cargo> _cargoToAdd;
	// When player adds more cargo to ship, cargo is added here. When outfitting is finished, cargo is transferred to the ship 
	private Ship _ship;
	private GridContainer _cargoGrid;


	public override void _Ready()
	{
		Signals.Connect(nameof(Signals.CargoSelectedForOutfit), new Callable(this, nameof(SaveCargo)));
	}

	public void Init(Ship ship)
	{
		_cargoToAdd = new List<Cargo>(ship.GetCargo());
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
		
		if (!_ship.IsCargoFull())
			_cargoGrid.AddChild(_buildAddCargoButton());
	}

	/**Returns the amount of cargo that will be in the ship after outfitting*/
	public int GetCargoCount()
	{
		return _cargoToAdd.Count;
	}
	
	
	

	/** Sets the cargo of the ship to be the selected cargo */
	public void SendCargo(Ship ship)
	{
		ship.SetCargo(_cargoToAdd);
	}
	
	private MarginContainer _buildCargoButton(Cargo cargo)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/OutfitCargoButton.tscn");
		var inst = (MarginContainer)scene.Instantiate();
		var button = inst.GetNode<TextureButton>("PanelCont/TextureButton");

		button.TextureNormal = (Texture2D)GD.Load(cargo.GetImagePath());
		button.TextureHover = (Texture2D)GD.Load("res://Assets/Icons/change.png");
		// TODO: Manage cargo tooltip
		
		//button.Pressed += () => CargoButtonPressed(); For now can't do anything with existing cargo
		return inst;
	}
	
	private MarginContainer _buildAddCargoButton()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/OutfitCargoButton.tscn");
		var inst = (MarginContainer)scene.Instantiate();
		var button = inst.GetNode<TextureButton>("PanelCont/TextureButton");

		button.TextureNormal = (Texture2D)GD.Load("res://Assets/Icons/empty.png");
		button.TextureHover = (Texture2D)GD.Load("res://Assets/Icons/plus.png");
		
		button.Pressed += AddButtonPressed;
		return inst;
	}

	private void AddButtonPressed()
	{
		Signals.EmitSignal(nameof(Signals.AddCargoClicked));
		Signals.EmitSignal(nameof(Signals.SimpleButtonClicked));
	}

	public void SaveCargo(string cargoName)
	{
		_cargoToAdd.Add(game_state.PopCargo(cargoName));
		Signals.EmitSignal(nameof(Signals.CargoAdded));
	}

	public void AddCargoToShip(string cargoName)
	{
		_ship.AddCargo(game_state.PopCargo(cargoName));
		Signals.EmitSignal(nameof(Signals.CargoAdded));
	}
}
