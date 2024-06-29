using GloryOfRitiria.Scripts.Events;
using GloryOfRitiria.Scripts.Global;
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
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		_leftEventButton = GetNode<Button>("VBox/HBoxEventHandling/MBoxLeft/ButtonLeft");
		_leftEventButton.Pressed += () => {
			_signals.EmitSignal(nameof(_signals.NextEventButtonClicked));
			_previousEvent();
		};
		_leftEventButton.Disabled = true;
		
		_rightEventButton = GetNode<Button>("VBox/HBoxEventHandling/MBoxRight/ButtonRight");
		_rightEventButton.Pressed += () => {
			_signals.EmitSignal(nameof(_signals.NextEventButtonClicked));
			_nextEvent();
		};
		_rightEventButton.Disabled = game_state.EventsForTurn.Count <= 1; // disabled if only one event
		
		var exitButton = GetNode<Button>("VBox/HBoxTitle/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.EventWindowClosed));
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
		};

		// Show the first event
		_setDisplayedEvent();
		
		// Disable all other buttons while event window is active
		GetTree().Paused = true;
	}

	// Set the event displayed based on _eventIndex
	private void _setDisplayedEvent()
	{
		if (game_state.EventsForTurn.Count > 0 && _eventIndex >= 0 && _eventIndex < game_state.EventsForTurn.Count)
			_buildEventWindow(game_state.EventsForTurn[_eventIndex]);
		else GD.Print("Tried displaying non existing event or event list empty");
	}

	private void _nextEvent(bool deactivateLeft = false)
	{
		_eventIndex += 1;
		if (deactivateLeft)
			_leftEventButton.Disabled = true;
		else 
			_leftEventButton.Disabled = false; 
		
		if (_eventIndex >= game_state.EventsForTurn.Count-1) 
			_rightEventButton.Disabled = true; // if no event right of the new one
		
		_setDisplayedEvent();
	}

	private void _previousEvent(bool deactivateRight = false)
	{
		_eventIndex -= 1;
		// If after moving to left, we're still on last event
		if (deactivateRight)
			_rightEventButton.Disabled = true;
		else
			_rightEventButton.Disabled = false; 
		
		if (_eventIndex == 0)
			_leftEventButton.Disabled = true; // if on first event in the list
		
		_setDisplayedEvent();
	}


	private void _buildEventWindow(GameEvent ev)
	{
		// Remove currently displayed event if exists
		_hideDisplayedEvent();
		
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
		
		// I use the a class for option info and methods and set up a new button with all its information
		var optionsBox = eventInst.GetNode<VBoxContainer>("MBox/VBox/OptionsMBox/OptionsVBox");
		
		// Remove the default buttons
		foreach (var child in optionsBox.GetChildren()) child.QueueFree();
		
		
		foreach (var choice in ev.Options)
		{
			var choiceInst = (EventChoice) GD.Load<PackedScene>("res://Scenes/Parts/ChoiceButton.tscn").Instantiate();
			choiceInst.Text = choice.Desc;
			choiceInst.TextOnTooltip = choice.Desc;
			choiceInst.Pressed += () =>
			{
				choice.ApplyEffects();
				_endEvent(ev);
				_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
				_signals.EmitSignal(nameof(_signals.EventOptionButtonClicked));
			};
			optionsBox.AddChild(choiceInst);
		}
		
		// Add the instance to the event container
		var eventBoxInstance = GetNode<MarginContainer>("VBox/HBoxEventHandling/MBoxEvents");
		eventBoxInstance.AddChild(eventInst);
	}

	// Removes an event permanently, should be called when player chooses an option in an event
	private void _endEvent(GameEvent ev)
	{
		// If it was the last event, we simply destroy the current event
		if (game_state.EventsForTurn.Count == 1) _hideDisplayedEvent();
		
		// If we were at the last event in chain, but there are others before, we go the the previous one
		else if (_eventIndex == game_state.EventsForTurn.Count - 1)
		{
			_rightEventButton.Disabled = true;
			_previousEvent(true);
		}
		// If there were more than one event and it wasn't the last in chain, we go forward in chain
		else 
		{
			if (_eventIndex == 0) // If we were on the first event, left should stay disabled
			{
				_nextEvent(true);
				_eventIndex = 0; // Next event became new first event
			}
			else _nextEvent(); // If we weren't on first event, there will still be some events to the left
		}
		
		game_state.EventsForTurn.Remove(ev);

		// If after removing the event we're outside the events range
		if (_eventIndex >= game_state.EventsForTurn.Count)
			_eventIndex = game_state.EventsForTurn.Count - 1;
	}

	// Remove current event from being displayed without removing it from event list
	private void _hideDisplayedEvent()
	{
		// Remove the displayed event
		// Should either run once or never
		foreach (var eventInstance in GetNode<MarginContainer>("VBox/HBoxEventHandling/MBoxEvents").GetChildren())
		{
			if (eventInstance != null) eventInstance.QueueFree(); // Only hide if event exists
		}
	}
}
