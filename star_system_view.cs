using Godot;
using System;

public partial class star_system_view : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var menuButton = GetNode<Button>("MenuButton");
		var planetButton = GetNode<TextureButton>("PlanetButton");
		menuButton.Pressed += MenuButtonPressed;
		planetButton.Pressed += PlanetButtonPressed;
		
		var label = GetNode<Label>("Label");
		label.Text = game_state.CurrentYear;
		game_state.CurrentYear = "ByeWorld";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void MenuButtonPressed()
	{
		GetTree().ChangeSceneToFile("./MainMenu.tscn");
	}
	
	private void PlanetButtonPressed()
	{
		GetTree().ChangeSceneToFile("./planet_game_scene.tscn");
	}
}
