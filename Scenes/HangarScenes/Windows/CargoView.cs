using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class CargoView : PanelContainer
{
	public GlobalSignals Signals;
	private List<Cargo> _shipCargo;
	// When player adds more cargo to ship, cargo is added here. When outfitting is finished, cargo is transferred to the ship 
	private Ship _ship;
	

	public override void _Ready()
	{
		_shipCargo = new List<Cargo>();
		
		Signals.Connect(nameof(Signals.CargoSelectedForOutfit), new Callable(this, nameof(AddCargoToShip)));
	}

	public void Init(Ship ship)
	{
		_shipCargo = ship.ShipCargo;
		_ship = ship;
		var cargoGrid = GetNode<GridContainer>("GridCont");

		foreach (var cargo in ship.ShipCargo)
		{
			cargoGrid.AddChild(_buildCargoButton(cargo));
		}
		
		cargoGrid.AddChild(_buildAddCargoButton());
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
		
		button.Pressed += () => AddButtonPressed();
		return inst;
	}

	private void AddButtonPressed()
	{
		Signals.EmitSignal(nameof(Signals.AddCargoClicked));
		Signals.EmitSignal(nameof(Signals.SimpleButtonClicked));
	}

	public void AddCargoToShip(string cargoName)
	{
		_ship.ShipCargo.Add(game_state.PopCargo(cargoName));
		Signals.EmitSignal(nameof(Signals.CargoAdded));
	}
}
