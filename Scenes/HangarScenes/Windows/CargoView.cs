using System.Collections.Generic;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class CargoView : PanelContainer
{
	private GlobalSignals _signals;
	private List<Cargo> _shipCargo;
	// When player adds more cargo to ship, cargo is added here. When outfitting is finished, cargo is transferred to the ship 

	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_shipCargo = new List<Cargo>();
		_signals.Connect(nameof(_signals.OutfitWindowReady), new Callable(this, nameof(_initCargo)));
	}

	private void _initCargo(Ship ship)
	{
		_shipCargo = ship.ShipCargo;
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
		// TODO: Manage cargo tooltip
		
		//button.Pressed += () => CargoButtonPressed(); For now can't do anything with existing cargo
		return inst;
	}
	
	private MarginContainer _buildAddCargoButton()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/OutfitCargoButton.tscn");
		var inst = (MarginContainer)scene.Instantiate();
		var button = inst.GetNode<TextureButton>("PanelCont/TextureButton");

		button.TextureNormal = (Texture2D)GD.Load("res://Assets/Icons/plus.png");
		
		button.Pressed += () => AddButtonPressed();
		return inst;
	}

	private void AddButtonPressed()
	{
		_signals.EmitSignal(nameof(_signals.AddCargoClicked));
	}
}
