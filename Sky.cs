using Godot;
using System;

public partial class Sky : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var label = GetNode<Label>("Label");
		label.Text = game_state.CurrentYear;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			var clickPosition = GetGlobalMousePosition();
			var collisionShape = GetNode<CollisionPolygon2D>("SkyCollision");
			if (Geometry2D.IsPointInPolygon(clickPosition, collisionShape.Polygon))
			{
				GetTree().ChangeSceneToFile("./star_system_view.tscn");
			}
		}
	}
}
