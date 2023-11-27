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
	public List<GameEvent> getSatisfiedEvents()
	{
		var res = new List<GameEvent>();
		var attributeValues = game_state.GetAttributeValues();
		if (attributeValues.TryGetValue("Res1", out var res1Value))
		{
			var res1Type = res1Value.Item1;
			var res1 = res1Value.Item2;

			GD.Print("res1 Type: " + res1Type);
			GD.Print("res1 Value: " + res1);
			// I manage to have this :
			// res1 Type: System.Int32
			// res1 Value: 30
			// So I can get the attribute name, value and type. But I need to make the type more... Universal and easy for the ifs
		}
		
		foreach (var gameEvent in _eventList)
		{
			var shouldBeAdded = true;
			foreach (var eventCond in gameEvent.Conditions)
			{
				// FieldInfo attribute = typeof(game_state).GetField(eventCond.VarName, BindingFlags.Public | BindingFlags.Static);
				// if (attribute is null) continue;
				// Type attributeType = attribute.GetType();
				//
				// var attributeValue = attribute.GetValue(_gameState);
				// var convertedValue = Convert.ChangeType(attributeValue, attributeType);
				//
				// if (convertedValue is not IComparable comparableValue) continue;
				//
				// switch (eventCond.CondType)
				// {
				// 	case ">":
				// 		if (!(comparableValue.CompareTo(Convert.ChangeType(eventCond.Value, attributeType)) > 0) )
				// 		{
				// 			shouldBeAdded = false;
				// 		}
				// 		break;
				// }
				//
				// if (!shouldBeAdded) break;
			}
		}
		return res;
	}
}