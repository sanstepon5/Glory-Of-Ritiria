using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

public partial class event_manager : Node
{
	// Events should be popped out of the list if they're one time only
	private List<GameEvent> _eventList;
	private game_state _gameState;
	// Called once when game is opened
	public override void _Ready()
	{
		_gameState = GetNode<game_state>("/root/game_state");
		_eventList = new List<GameEvent>();
		
		var e = new GameEvent();
		e.Conditions.Add(new EventCondition("turn", "==", "3"));
		_eventList.Add(e);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// I should look into how to make "all should satisfy" and "at least one should satisfy"
	public List<GameEvent> getSatisfiedEvents()
	{
		var res = new List<GameEvent>();
		foreach (var gameEvent in _eventList)
		{
			var shouldBeAdded = true;
			foreach (var eventCond in gameEvent.Conditions)
			{
				FieldInfo attribute = typeof(game_state).GetField(eventCond.VarName);
				if (attribute is null) continue;
				Type attributeType = attribute.GetType();

				var attributeValue = attribute.GetValue(_gameState);
				var convertedValue = Convert.ChangeType(attributeValue, attributeType);

				if (convertedValue is not IComparable comparableValue) continue;
				
				switch (eventCond.CondType)
				{
					case ">":
						if (!(comparableValue.CompareTo(Convert.ChangeType(eventCond.Value, attributeType)) > 0) )
						{
							shouldBeAdded = false;
						}
						break;
				}

				if (!shouldBeAdded) break;
			}
			
		}
		return res;
	}
}