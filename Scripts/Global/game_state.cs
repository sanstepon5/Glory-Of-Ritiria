using Godot;
using System;
using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils;
using GloryOfRitiria.Scripts.Utils.Events;

// This is a singleton that's loaded in every scene and will contain persistent state of the game.
// It should be empty on main screen a be loaded when loading a save.
public partial class game_state : Node
{
	[Export] // If I want to save using a packed scene. It works only with "Variant vars", so no classes saved...
	public static string CurrentYear { set; get; } = "970 APE\n(2017 CE)";

	public static int CurrentTurn { set; get; } = 0;

	public static double Res1 { set; get; } = 0;
	public static double Res1Rate { set; get; } = 2.5;

	public static List<Flags> CurrentFlags = new List<Flags>();
	
	public static List<GameEvent> EventsForTurn = new List<GameEvent>();
	
	public static Dictionary<string, Tuple<Type, object>> GetAttributeValues()
	{
		var attributeValues = new Dictionary<string, Tuple<Type, object>>
		{
			{ "CurrentYear", Tuple.Create(typeof(string), (object)CurrentYear) },
			{ "CurrentTurn", Tuple.Create(typeof(int), (object)CurrentTurn) },
			{ "Res1", Tuple.Create(typeof(double), (object)Res1) },
			{ "Res1Rate", Tuple.Create(typeof(double), (object)Res1Rate) }
			// Add other attributes as needed...
		};

		return attributeValues;
	}
	
	// Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }

	public static void UpdateResources()
	{
		// Res 1
		Res1 += Res1Rate;
		if (Res1 < 0) Res1 = 0;
		// And others...
	}
	
	public static void SetCurrentYear()
	{
		// For now
		CurrentYear = "9" + (70 + CurrentTurn) + " APE\n(20" + (17 + CurrentTurn) + " CE)";
	}
	
}
