using System;
using System.Collections.Generic;
using Godot;

namespace GloryOfRitiria.Scripts.Scenes.Parts;

public partial class MultiEventWindow : Panel
{
	private GlobalSignals _signals;
	
	private int _eventIndex = 0;

	private Button _leftEventButton;
	private Button _rightEventButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		_leftEventButton = GetNode<Button>("VBox/HBoxEventHandling/MBoxLeft/ButtonLeft");
		_leftEventButton.Pressed += PreviousEvent;
		_leftEventButton.Disabled = true;
		
		_rightEventButton = GetNode<Button>("VBox/HBoxEventHandling/MBoxRight/ButtonRight");
		_rightEventButton.Pressed += NextEvent;
		_rightEventButton.Disabled = game_state._eventsForTurn.Count <= 1; // disabled if only one event
		
		var exitButton = GetNode<Button>("VBox/HBoxTitle/ExitButton");
		exitButton.Pressed += () =>
		{
			GetTree().Paused = false;
			QueueFree();
		};

		SetDisplayedEvent();
		
		// Disable all other buttons while event window is active
		GetTree().Paused = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Called once with signal on creation of the window
	// public void SetWindowEvents()
	// {
	// 	if (game_state._eventsForTurn.Count > 1) // if more than one event right button should become active
	// 	{
	// 		_rightEventButton.Disabled = false;
	// 	}
	// 	SetDisplayedEvent(); // Set the displayed event to the first in the list
	// }

	// Set the event displayed based on _eventIndex
	public void SetDisplayedEvent()
	{
		if (game_state._eventsForTurn.Count > 0 && _eventIndex >= 0 && _eventIndex < game_state._eventsForTurn.Count)
		{
			BuildEventWindow(game_state._eventsForTurn[_eventIndex]);
		}
		else GD.Print("Tried displaying non existing event or event list empty");
	}

	private void NextEvent()
	{
		_eventIndex += 1;
		_leftEventButton.Disabled = false; // If next event is called there is a previous event
		if (_eventIndex >= game_state._eventsForTurn.Count-1) _rightEventButton.Disabled = true; // if no event right of the new one
		
		SetDisplayedEvent();
	}

	private void PreviousEvent()
	{
		_eventIndex -= 1;
		_rightEventButton.Disabled = false; // If previous event is called there is a next event
		if (_eventIndex == 0) _leftEventButton.Disabled = true; // if on first event in the list
		
		SetDisplayedEvent();
	}
	
	
	public void BuildEventWindow(GameEvent ev)
	{
		// Remove currently displayed event if exists
		HideDisplayedEvent();
		
		// Load an event window instance
		var eventWindow = GD.Load<PackedScene>("res://Scenes/Parts/event_window.tscn");
		var eventInst = (Panel) eventWindow.Instantiate();
		
		// Set various fields of the event
		var title = eventInst.GetNode<RichTextLabel>("MBox/VBox/TitleHBox/TitleLabel");
		title.Text = ev.Name;
		
		var image = eventInst.GetNode<TextureRect>("MBox/VBox/ImageMBox/EventImage");
		image.Texture = (Texture2D)GD.Load(ev.ImagePath);
		
		var desc = eventInst.GetNode<RichTextLabel>("MBox/VBox/DescMBox/DescLabel");
		desc.Text = ev.Description;
		
		// Should add effects of options here
		var exitOption = eventInst.GetNode<Button>("MBox/VBox/OptionsMBox/OptionsVBox/DefaultButton");
		exitOption.Pressed += () => EndEvent(ev); // Remove event permanently on exit
		
		// Add the instance to the event container
		var eventBoxInstance = GetNode<MarginContainer>("VBox/HBoxEventHandling/MBoxEvents");
		eventBoxInstance.AddChild(eventInst);
	}

	// Removes an event permanently, should be called when player chooses an option in an event
	private void EndEvent(GameEvent ev)
	{
		// This section can surely could be refactored to use next/prevEvent()
		// And surely some of the ifs aren't necessary
		
		// If only one event left, just remove it
		if (game_state._eventsForTurn.Count == 1)
		{
			game_state._eventsForTurn.Remove(ev);
			HideDisplayedEvent();
		}
		
		// if not on last event, load next event
		else if (_eventIndex < game_state._eventsForTurn.Count - 1)
		{
			game_state._eventsForTurn.Remove(ev); // No need to increment index as we just removed an event
			
			if (_eventIndex != 0) _leftEventButton.Disabled = false; // if destroyed event wasn't the first one
			if (_eventIndex >= game_state._eventsForTurn.Count-1)  _rightEventButton.Disabled = true; // if new event is last
			
			SetDisplayedEvent();
		}
		
		// if on last event, load previous event
		else if (_eventIndex == game_state._eventsForTurn.Count - 1)
		{
			game_state._eventsForTurn.Remove(ev);
			_eventIndex -= 1; // Move to the left

			if (_eventIndex != 0) _leftEventButton.Disabled = false; // if destroyed event wasn't the first one
			if (_eventIndex >= game_state._eventsForTurn.Count - 1) _rightEventButton.Disabled = true; // if no event right of the new one
			
			SetDisplayedEvent();
		}
		else throw new IndexOutOfRangeException("Index of current event somehow out of range");
	}

	// Remove current event from being displayed without removing it from event list
	private void HideDisplayedEvent()
	{
		// Remove the displayed event
		// Should either run once or never
		foreach (var eventInstance in GetNode<MarginContainer>("VBox/HBoxEventHandling/MBoxEvents").GetChildren())
		{
			if (eventInstance != null) eventInstance.QueueFree(); // Only hide if event exists
		}
	}
}
