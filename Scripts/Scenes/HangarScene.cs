using Godot;
using System;
using GloryOfRitiria;
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


		if (slot.State is SlotState.Building or SlotState.Full)
		{
			var slotName = inst.GetNode<LineEdit>("MCont/VBox/NameHBox/MCont/TextEdit");
			slotName.Text = slot.Ship.Name;
			
			// If already has a ship, show ship's location
			var slotLocation = inst.GetNode<RichTextLabel>("MCont/VBox/LocationHBox/MarginContainer/LocationLabel");
			slotLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + slot.Ship.Location.Name;
		}

		if (slot.State == SlotState.Empty)
		{
			// On an empty slot, show the slot's location
			var slotLocation = inst.GetNode<RichTextLabel>("MCont/VBox/LocationHBox/MarginContainer/LocationLabel");
			slotLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + slot.Location.Name;
		}
		
		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		
		exitButton.Pressed += () =>
		{
			GetTree().Paused = false;
			windowCont.GetChild(0).QueueFree();
		};
		
		
		GetTree().Paused =  true;
		
		windowCont.AddChild(inst);
	}
}
