using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils;
using GloryOfRitiria.Scripts.Utils.Events;
using Godot;

// TODO: Add single fire events
namespace GloryOfRitiria.Scripts.Global;

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
			var eventParser = new EventFileParser(file);
			_eventList = eventParser.ReadEventList();
		}
		else
		{
			var err = FileAccess.GetOpenError();
			GD.Print(err);
		}
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