using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using GloryOfRitiria.Scripts.Utils;

public partial class event_manager : Node
{
	// Events should be popped out of the list if they're one time only
	private List<GameEvent> _eventList;
	// Called once when game is opened
	public override void _Ready()
	{
		_eventList = new List<GameEvent>();

		var e = new GameEvent();
		e.SetSingleCondition("CurrentTurn", "==", "3");
		_eventList.Add(e);
	}
	
	public override void _Process(double delta)
	{
	}

	// I should look into how to make "all should satisfy" and "at least one should satisfy"
	// Here it's "For All"
	public List<GameEvent> getSatisfiedEvents()
	{
		var res = new List<GameEvent>();
		foreach (var gameEvent in _eventList)
		{
			if (gameEvent.IsSatisfied())
			{
				res.Add(gameEvent);
			}
		}
		return res;
	}

}