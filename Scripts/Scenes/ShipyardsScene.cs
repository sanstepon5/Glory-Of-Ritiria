using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

public partial class ShipyardsScene : Node2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.Connect(nameof(_signals.ConstructionWindowRequested), new Callable(this, nameof(BuildConstructionWindow)));
		
		_signals.Connect(nameof(_signals.FullSlotClicked), new Callable(this, nameof(BuildOutfittingWindow)));


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
			GetTree().Paused = false;
			windowCont.GetChild(0).QueueFree();
		};
		
		var buildButton = inst.GetNode<Button>("MCont/VBox/ButtonMargin/BuildButton");
		buildButton.Pressed += () =>
		{
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
		var inst = (PanelContainer)scene.Instantiate();
		
		var shipNameLabel = inst.GetNode<RichTextLabel>("MCont/VBox/NameHBox/MCont/ShipName");
		shipNameLabel.Text = ship.Name;
		
		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			GetTree().Paused = false;
			windowCont.GetChild(0).QueueFree();
		};
		
		var outfitButton = inst.GetNode<Button>("MCont/VBox/ButtonMargin/OutfitButton");
		outfitButton.Pressed += () =>
		{
			GetTree().Paused = false;
			windowCont.GetChild(0).QueueFree();
			
			// Ship.blablabla = blablabla			
		};
		
		GetTree().Paused =  true;
		
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
}
