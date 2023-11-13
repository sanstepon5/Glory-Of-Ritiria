using Godot;
using System;

public partial class base_game_scene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		updateSceneLabels();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// Called on each input, maybe would be better to move it to root of scene and check for all children ? Not sure
	public override void _Input(InputEvent @event)
	{
		// Should replace Pressed by when the button is up, else you can hold the click...
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			var clickPosition = GetGlobalMousePosition();
			var collisionShape = GetNode<CollisionPolygon2D>("NextTurnButton/AreaCollision");
			if (Geometry2D.IsPointInPolygon(clickPosition, collisionShape.Polygon))
			{
				GD.Print("clicked!");
				game_state.CurrentTurn += 1;
				game_state.setCurrentYear();
				updateSceneLabels();
			}
		}
	}

	public void updateSceneLabels()
	{
		var yearLabel = GetNode<Label>("TopBar/CurrentYear");
		yearLabel.Text = game_state.CurrentYear;

		var turnNumber = GetNode<Label>("NextTurnButton/CurrentTurn");
		turnNumber.Text = "Current Turn: " + game_state.CurrentTurn;
	}
}
