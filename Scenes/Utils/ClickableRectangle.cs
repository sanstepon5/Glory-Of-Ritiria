using Godot;
using GloryOfRitiria;

// Works only for simple parameterless signals
public partial class ClickableRectangle : Control
{
	[Export] public NodePath Parent; 
	[Export] public string SignalOnClick;
	[Export] public string SignalParameter = "";
	
	private GlobalSignals _signals;
	private Control _parentNode;


	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_parentNode = (Control) GetNode(Parent);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseEvent) return;
		
		if (!mouseEvent.Pressed && mouseEvent.ButtonIndex is MouseButton.Left) // !Pressed is Released
		{
			var clickPosition = GetGlobalMousePosition();
			if (!HasPoint(GlobalPosition, _parentNode.Size, clickPosition)) return;

			if (SignalParameter == "")
				_signals.EmitSignal(SignalOnClick);
			else
				_signals.EmitSignal(SignalOnClick, SignalParameter);
		}
	}

	// Check if the clicked point is within the boundaries of a control node
	public static bool HasPoint(Vector2 globalPosition, Vector2 size, Vector2 point)
	{
		var rect = new Rect2(globalPosition, size);
		return rect.HasPoint(point);
	}

}
