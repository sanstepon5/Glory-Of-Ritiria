using Godot;
using System;

// This is a singleton that's loaded in every scene and will contain persistent state of the game.
// It should be empty on main screen a be loaded when loading a save.
public partial class game_state : Node
{
	[Export] // If I want to save using a packed scene. It works only with "Variant vars", so no classes saved...
	public static string CurrentYear { set; get; } = "970 APE\n(2017 CE)";

	public static int CurrentTurn { set; get; }
	
	// Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }

	public static void setCurrentYear()
	{
		// For now
		CurrentYear = "9" + (70 + CurrentTurn) + " APE\n(20" + (17 + CurrentTurn) + "CE)";
	}
}
