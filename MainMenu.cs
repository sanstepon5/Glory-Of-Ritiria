using Godot;
using System;
using Godot.Collections;
using Microsoft.VisualBasic;

public partial class MainMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var playButton = GetNode<Button>("VFlowContainer/MarginContainer/PlayButton");
		var optionsButton = GetNode<Button>("VFlowContainer/MarginContainer2/OptionsButton");
		var quitButton = GetNode<Button>("VFlowContainer/MarginContainer3/QuitButton");
		playButton.Pressed += PlayButtonPressed;
		optionsButton.Pressed += OptionsButtonPressed;
		quitButton.Pressed += QuitButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PlayButtonPressed()
	{
		LoadGameState();
		GetTree().ChangeSceneToFile("./base_game_scene.tscn");
	}
	//TODO
	private void OptionsButtonPressed()
	{
		
	}
	private void QuitButtonPressed()
	{
		GetTree().Quit();
	}

	// Loads data of gameState from a save file
	private static void LoadGameState()
	{
		game_state.CurrentYear = "970 APE\n(2017 CE)";
	}

	// TODO
	private Dictionary<string, Variant> ReadSaveFile()
	{
		// Creates the helper class to interact with JSON
		var json = new Json();
		
		var saveFile = FileAccess.Open("user://file.json", FileAccess.ModeFlags.Read);
		var saveData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
		
		// Read the json file line by line
		while (saveFile.GetPosition() < saveFile.GetLength())
		{
			var jsonString = saveFile.GetLine();
			
			var parseResult = json.Parse(jsonString);
			if (parseResult != Error.Ok)
			{
				GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
				continue;
			}

			// Get the data from the JSON object
			
			
			
		}
		return saveData;
	}
}
