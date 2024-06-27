using Godot;
using System;

public partial class MapCamera : Camera2D
{
	[Export] public NodePath OwnerPath;
	
	[Export] public Vector2 ZoomSpeed = new (0.05f, 0.05f);
	[Export] public Vector2 MinZoom = new (0.5f, 0.5f);
	[Export] public Vector2 MaxZoom = new (1.5f, 1.5f);

	[Export] public bool CustomCorner = false;
	[Export] public Vector2 MostLeftHighPoint = new (0f, 0f);
	[Export] public Vector2 MostRightLowPoint = new (1920f, 1080f);
	
	[Export] public float DragSensitivity = 2.0f;
	
	private Vector2 _viewportSize;


	public override void _Ready()
	{
		// Screen size, normally
		_viewportSize = GetViewportRect().Size;
		if (!CustomCorner)
		{
			MostRightLowPoint = _viewportSize;
		}
	}

	public override void _Input(InputEvent @event)
	{
		// Drag the camera when left button is pressed and dragged
		if (@event is InputEventMouseMotion mouseMotion && Input.IsMouseButtonPressed(MouseButton.Left))
		{
			Position -= mouseMotion.Relative * DragSensitivity / Zoom;
			
			Position = Position.Clamp(MostLeftHighPoint, MostRightLowPoint);
			
		}

		// Zoom in and zoom out the camera, limited by min and max values
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.WheelUp) 
				Zoom += ZoomSpeed * Zoom;
			else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
				Zoom -= ZoomSpeed * Zoom;
			
			Zoom = Zoom.Clamp(MinZoom, MaxZoom);
		}
	}
}
