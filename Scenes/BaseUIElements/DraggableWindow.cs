using Godot;

[GlobalClass]
public partial class DraggableWindow : PanelContainer
{
	private bool _dragging = false;
	private Vector2 _dragOffset;

	[Export] public Control TitleBar; // assign in the inspector

	public override void _Ready()
	{
		// Connect signals via code
		TitleBar.GuiInput += OnTitleBarGuiInput;
	}

	private void OnTitleBarGuiInput(InputEvent @event)
	{
		GD.Print("OnTitleBarGuiInput");
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex != MouseButton.Left) return;
			if (mouseButton.Pressed)
			{
				GD.Print("StartDrag");
				_dragging = true;
				_dragOffset = GetGlobalMousePosition() - GlobalPosition;
			}
			else
			{
				GD.Print("EndDrag");
				_dragging = false;
			}
		}
		
		else if (@event is InputEventMouseMotion mouseMotion && _dragging)
		{
			GD.Print("Dragging...");
			GlobalPosition = GetGlobalMousePosition() - _dragOffset;
		}
	}
}
