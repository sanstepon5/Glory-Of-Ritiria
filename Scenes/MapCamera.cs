using Godot;
using System;

public partial class MapCamera : Camera2D
{
	[Export] public Vector2 ZoomSpeed = new (0.05f, 0.05f);
	[Export] public Vector2 MinZoom = new (0.1f, 0.1f);
	[Export] public Vector2 MaxZoom = new (2f, 2f);
	[Export] public float DragSensitivity = 2.0f;

	public override void _Input(InputEvent @event)
	{
		// Drag the camera when left button is pressed and dragged
		if (@event is InputEventMouseMotion mouseMotion && Input.IsMouseButtonPressed(MouseButton.Left))
			Position -= mouseMotion.Relative * DragSensitivity / Zoom;

		// Zoom in and zoom out the camera, limited by min and max values
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.WheelUp) 
				Zoom += ZoomSpeed * Zoom;
			else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
				Zoom -= ZoomSpeed * Zoom;
			
			Zoom.Clamp(MinZoom, MaxZoom);
		}
	}
}
