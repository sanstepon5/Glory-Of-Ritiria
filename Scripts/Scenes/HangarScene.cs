using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

public partial class HangarScene : Node2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.Connect(nameof(_signals.ConstructionWindowRequested), new Callable(this, nameof(BuildConstructionWindow)));
	}

	public void BuildConstructionWindow(ShipConstructionSlot slot)
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/ShipConstructionWindow.tscn");
		var inst = (PanelContainer)scene.Instantiate();

		var shipName = inst.GetNode<LineEdit>("MCont/VBox/NameHBox/MCont/TextEdit");
		var constructionLocation = inst.GetNode<RichTextLabel>("MCont/VBox/LocationHBox/MarginContainer/LocationLabel");

		if (slot.State is SlotState.Building or SlotState.Full)
		{
			shipName.Text = slot.Ship.Name;
			// If already has a ship, show ship's location
			constructionLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + slot.Ship.Location.Name;
		}

		if (slot.State == SlotState.Empty)
		{
			// On an empty slot, show the slot's location
			constructionLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + slot.Location.Name;
		}
		
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

			var ship = new Ship(shipName.Text, slot.Location, true);
			var newSlot = new ShipConstructionSlot(slot.Location, ship);
			game_state.ShipConstructionSlots.Add(newSlot);
			
			_signals.EmitSignal(nameof(_signals.ShipBuildStarted));
		};
		
		
		GetTree().Paused =  true;
		
		windowCont.AddChild(inst);
	}
}
