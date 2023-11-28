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
		// We need to wait until _gameState node is available before initializing
		//SetProcess(false);
		_gameState = GetNode<game_state>("/root/GameState");
		_eventList = new List<GameEvent>();

		var e = new GameEvent();
		e.Conditions.Add(new EventCondition("CurrentTurn", "==", "3"));
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
			var shouldBeAdded = true;
			foreach (var eventCond in gameEvent.Conditions)
			{
				var attributeValues = game_state.GetAttributeValues();
				if (!attributeValues.TryGetValue(eventCond.VarName, out var attribute)){
					continue;
				}
				var attributeType = attribute.Item1;
				var attributeVal = attribute.Item2;

				GD.Print("attribute Type: " + attributeType.Name);
				GD.Print("attribute Value: " + attributeVal);
				switch (eventCond.CondType)
				{
					case "==":
						if (attributeVal == Convert.ChangeType(eventCond.Value, attributeType) )
						{
							GD.Print("True!");
							res.Add(gameEvent);
						}
						else shouldBeAdded = false;
						break;
					
					case "<":
					// Both are objects...
					// So can't compare. And can't cast because don't know type...
						if (attributeVal < Convert.ChangeType(eventCond.Value, attributeType) )
						{
							GD.Print("True!");
							res.Add(gameEvent);
						}
						else shouldBeAdded = false;
						break;
				}
				
				if (!shouldBeAdded) break;
			}
		}
		return res;
	}

}