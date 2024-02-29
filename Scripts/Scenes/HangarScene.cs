using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Utils;

public partial class HangarScene : Node2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.Connect(nameof(_signals.ConstructionWindowRequested), new Callable(this, nameof(BuildConstructionWindow)));
		_signals.EmitSignal(nameof(_signals.ShipyardsSceneOpened));
	}

	public void BuildConstructionWindow(Shipyard shipyard)
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/ShipConstructionWindow.tscn");
		var inst = (PanelContainer)scene.Instantiate();

		var shipName = inst.GetNode<LineEdit>("MCont/VBox/NameHBox/MCont/TextEdit");
		var constructionLocation = inst.GetNode<RichTextLabel>("MCont/VBox/LocationHBox/MarginContainer/LocationLabel");

		// if (shipyard.State is SlotState.Busy /*or SlotState.Full*/)
		// {
		// 	shipName.Text = shipyard.Ship.Name; // If already has a ship, show ship's location
		// 	constructionLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + shipyard.Ship.Location.Name;
		// }
		constructionLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + shipyard.Location.Name;
		
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

			var ship = new Ship(shipName.Text, shipyard.Location, true);
			shipyard.StartConstruction(ship);
			
			_signals.EmitSignal(nameof(_signals.ShipBuildStarted));
		};
		
		
		GetTree().Paused =  true;
		
		windowCont.AddChild(inst);
	}
}
