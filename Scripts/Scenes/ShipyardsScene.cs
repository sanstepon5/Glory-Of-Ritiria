using GloryOfRitiria.Scenes.HangarScenes.Windows;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

namespace GloryOfRitiria.Scripts.Scenes;

public partial class ShipyardsScene : Node2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.Connect(nameof(_signals.ConstructionWindowRequested), new Callable(this, nameof(BuildConstructionWindow)));
		
		_signals.Connect(nameof(_signals.FullSlotClicked), new Callable(this, nameof(BuildOutfittingWindow)));
		_signals.Connect(nameof(_signals.AddCargoClicked), new Callable(this, nameof(BuildCargoSelectWindow)));
		
		
		_signals.Connect(nameof(_signals.ThirdLevelProcessEntered), new Callable(this, nameof(DisableRightWindowProcess)));
		_signals.Connect(nameof(_signals.ThirdLevelProcessExited), new Callable(this, nameof(EnableRightWindowProcess)));
		
		_signals.Connect(nameof(_signals.CargoSelectedForOutfit), new Callable(this, nameof(ClearCenterWindow)));


		InitBodiesSelectionMenu();
		_signals.EmitSignal(nameof(_signals.ShipyardsSceneOpened));
	}

	public void BuildConstructionWindow(Shipyard shipyard)
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/ShipConstructionWindow.tscn");
		var inst = (PanelContainer)scene.Instantiate();
		
		var constructionLocation = inst.GetNode<RichTextLabel>("MCont/VBox/LocationHBox/MarginContainer/LocationLabel");
		constructionLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + shipyard.Location.Name;
		
		// var costLabel = inst.GetNode<RichTextLabel>("MCont/VBox/TimeCostHBox/MarginContainer/CostLabel");
		// costLabel.Text = shipyard.TurnCost + "  [img]res://Assets/GUI/Icons/32/time.png[/img]";
		
		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			GetTree().Paused = false;
			windowCont.GetChild(0).QueueFree();
		};
		
		var buildButton = inst.GetNode<Button>("MCont/VBox/ButtonMargin/BuildButton");
		buildButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			GetTree().Paused = false;
			windowCont.GetChild(0).QueueFree();
			
			var shipName = inst.GetNode<LineEdit>("MCont/VBox/NameHBox/MCont/TextEdit");
			var ship = new Ship(shipName.Text, shipyard.Location, true);
			shipyard.StartConstruction(ship);
			
			_signals.EmitSignal(nameof(_signals.ShipBuildStarted));
			
		};
		
		
		GetTree().Paused =  true;
		
		windowCont.AddChild(inst);
	}	
	
	public void BuildOutfittingWindow(Ship ship)
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/ShipOutfittingWindow.tscn");
		var inst = (ShipOutfittingWindow)scene.Instantiate();
		inst.Init(ship, _signals);
		
		windowCont.AddChild(inst);
	}
	
	public void BuildCargoSelectWindow()
	{
		var windowCont = GetNode<ReferenceRect>("CenterWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/CargoSelectWindow.tscn");
		var inst = (PanelContainer)scene.Instantiate();
		
		var cargoVBox = inst.GetNode<VBoxContainer>("VBox/MarginCont/ScrollCont/VBox");
		
		var exitButton = inst.GetNode<Button>("VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.ThirdLevelProcessExited));
			windowCont.GetChild(0).QueueFree();
		};

		_signals.EmitSignal(nameof(_signals.ThirdLevelProcessEntered));
		windowCont.AddChild(inst);
	}
	

	public void InitBodiesSelectionMenu()
	{
		// I shouldn't access a scene children here...
		var bodiesHBox = GetNode<HBoxContainer>("ShipyardSelectionMenu/HBoxContainer");
		foreach (var body in game_state.BodiesWithShipyards)
		{
			var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/HangarPlanetSelectionScene.tscn");
			var inst = (HangarPlanetSelectionScene)scene.Instantiate();
			inst.Body = body;
			bodiesHBox.AddChild(inst);
		}
	}

	public void DisableRightWindowProcess()
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		windowCont.ProcessMode = ProcessModeEnum.Disabled;
	}
	public void EnableRightWindowProcess()
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		windowCont.ProcessMode = ProcessModeEnum.WhenPaused;
	}

	public void ClearCenterWindow(string foo) //TODO: very messy...
	{
		var windowCont = GetNode<ReferenceRect>("CenterWindow");
		windowCont.GetChild(0).QueueFree();
		_signals.EmitSignal(nameof(_signals.ThirdLevelProcessExited));
	}
	
}
