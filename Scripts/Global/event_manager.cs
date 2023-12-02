using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
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

		// Event with a single condition
		var e = new GameEvent();
		e.SetSingleCondition("CurrentTurn", "==", "3");
		_eventList.Add(e);
		
		// Event with a For All condition that has two singular conditions
		var e2 = new GameEvent(); e2.Id = "Second_Event"; e2.Name = "Second Event Baby";
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