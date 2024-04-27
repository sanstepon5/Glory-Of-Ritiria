using Godot;

namespace GloryOfRitiria.Scenes.Utils;

public partial class ExtButton : Button
{

	[Signal] public delegate void LeftPressedEventHandler();
	[Signal] public delegate void RightPressedEventHandler();

	private bool _delayActive;
	private double _delay;
	
	public override void _Ready()
	{
		_delayActive = false;
		_delay = 0;
	}

	public override void _Process(double delta)
	{
		if (!_delayActive) return;

		if (_delay < 0.5)
		{
			_delay += delta;
			return;
		}
		
		_delay = 0;
		_delayActive = false;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseEvent || !@event.IsPressed()) return;
		if (_delayActive) return;
		
		switch (mouseEvent.ButtonIndex)
		{
			case MouseButton.Left:
				EmitSignal(nameof(LeftPressed));
				_delayActive = true;
				
				break;
			case MouseButton.Right:
				EmitSignal(nameof(RightPressed));
				_delayActive = true;

				break;
		}
	}
	
}
