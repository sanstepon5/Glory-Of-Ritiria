using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using GloryOfRitiria.Scripts.Scenes.Parts;
using GloryOfRitiria.Scripts.Utils;
using GloryOfRitiria.Scripts.Utils.Events;

public partial class event_manager : Node
{
	// Events should be popped out of the list if they're one time only
	private List<GameEvent> _eventList;
	// Called once when game is opened
	public override void _Ready()
	{
		_eventList = new List<GameEvent>();
		
		// Test of file opening both in editor and in build
		var file = FileAccess.Open(game_state.AssetsDir + "Events.txt", FileAccess.ModeFlags.Read);

		if (file != null)
		{
			var eventFileText = file.GetAsText();
			var eventParser = new EventFileParser(file);
			_eventList = eventParser.ReadEventList();
			
		}
		else
		{
			var err = FileAccess.GetOpenError();
			GD.Print(err);
		}


		//_addEvents();
	}

	private void _addEvents()
	{
		// Event with a single condition and a single option
		var e = new GameEvent();
		e.SetSingleCondition("CurrentTurn", "==", "3");
		var option1 = new Choice("option1","Woohoo, +10 Res1!");
		option1.Effects.Add(new Effect("AddRes1", "10"));
		e.Options.Add(option1);
		_eventList.Add(e);

		// Event with same single condition but with two options
		var eCopy = new GameEvent
		{
			Name = "Oh it's the second event!!!",
			Description = "This is really exciting!"
		};
		eCopy.SetSingleCondition("CurrentTurn", "==", "3");
		var option2 = new Choice("option2","Oh no, -20 Res1!");
		option2.Effects.Add(new Effect("AddRes1", "-20"));
		var option3 = new Choice("option3","Something mysterious will happen in the future!");
		option3.Effects.Add(new Effect("AddFlag", "RandomEvent1"));
		eCopy.Options.Add(option2);
		eCopy.Options.Add(option3);
		_eventList.Add(eCopy);

		// Event with a For All condition that has two singular conditions
		var e2 = new GameEvent
		{
			Id = "Second_Event",
			Name = "Second Event Baby"
		};
		var e2Cond = new ForAllCondition();
		e2Cond.AddCondition(new SingleCondition("CurrentTurn", ">", "2"));
		e2Cond.AddCondition(new SingleCondition("Res1", ">", "30"));
		e2.Condition = e2Cond;
		_eventList.Add(e2);
	}

	public override void _Process(double delta)
	{
	}
	
	public List<GameEvent> GetSatisfiedEvents()
	{
		var res = new List<GameEvent>();
		foreach (var gameEvent in _eventList)
		{
			if (gameEvent.IsSatisfied()) res.Add(gameEvent);
		}
		return res;
	}

}