using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class ShipConstructionWindow : PanelContainer
{
	private Shipyard _shipyard;

	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		GetTree().Paused =  true;
	}
		
	public void Init(Shipyard shipyard, GlobalSignals signals)
	{
		_signals = signals;
		_shipyard = shipyard;
		
		var constructionLocation = GetNode<RichTextLabel>("MCont/VBox/LocationHBox/MarginContainer/LocationLabel");
		constructionLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + _shipyard.Location.Name;
		
		// var costLabel = GetNode<RichTextLabel>("MCont/VBox/TimeCostHBox/MarginContainer/CostLabel");
		// costLabel.Text = _shipyard.TurnCost + "  [img]res://Assets/GUI/Icons/32/time.png[/img]";
		
		var exitButton = GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			GetTree().Paused = false;
			QueueFree();
		};
		
		var buildButton = GetNode<Button>("MCont/VBox/ButtonMargin/BuildButton");
		buildButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			
			var shipName = GetNode<LineEdit>("MCont/VBox/NameHBox/MCont/TextEdit");
			var ship = new Ship(shipName.Text, _shipyard.Location, true);
			_shipyard.StartConstruction(ship);
			
			_signals.EmitSignal(nameof(_signals.ShipBuildStarted));
			
			GetTree().Paused = false;
			QueueFree();
		};
	}
}
