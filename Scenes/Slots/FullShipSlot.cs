using Godot;
using GloryOfRitiria;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

public partial class FullShipSlot : PanelContainer
{
	private GlobalSignals _signals;
	public Ship Ship;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseEvent) return;
		
		if (!mouseEvent.Pressed && mouseEvent.ButtonIndex is MouseButton.Left) // !Pressed is Released
		{
			var clickPosition = GetGlobalMousePosition();
			if (!ClickableRectangle.HasPoint(GlobalPosition, Size, clickPosition)) return;

			_signals.EmitSignal(nameof(_signals.FullSlotClicked), Ship);
		}
	}
}
