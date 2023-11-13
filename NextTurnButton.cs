using Godot;
using System;

public partial class NextTurnButton : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// // Called on each input, maybe would be better to move it to root of scene and check for all children ? Not sure
	// public override void _Input(InputEvent @event)
	// {
	// 	if (Input.IsMouseButtonPressed(MouseButton.Left))
	// 	{
	// 		var clickPosition = GetGlobalMousePosition();
	// 		var collisionShape = GetNode<CollisionPolygon2D>("AreaCollision");
	// 		if (Geometry2D.IsPointInPolygon(clickPosition, collisionShape.Polygon))
	// 		{
	// 			game_state.CurrentTurn += 1;
	// 			game_state.setCurrentYear();
	// 		}
	// 	}
	// }
}
