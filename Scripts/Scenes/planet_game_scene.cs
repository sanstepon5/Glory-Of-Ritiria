using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scripts.Scenes;

public partial class planet_game_scene : Node2D
{
	private GlobalSignals _signals;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		//_gameState = GetNode<game_state>("/root/GameState");
		var incButton = GetNode<Button>("IncRes1Button");
		incButton.Pressed += OnIncButtonPressed;
		
		var phoneButton = GetNode<MenuButton>("PhoneButton");
		phoneButton.GetPopup().Connect("id_pressed", new Callable(this, nameof(_popupProcess)));
	}


	private void _popupProcess(int id)
	{
		switch (id)
		{
			case 0: _signals.EmitSignal(nameof(_signals.ShipyardsButtonClicked)); break;
			case 1: _signals.EmitSignal(nameof(_signals.FleetBureauButtonClicked)); break;
			default: GD.Print("Err"); break;
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			var clickPosition = GetGlobalMousePosition();
			var collisionShape = GetNode<CollisionPolygon2D>("Sky/SkyCollision");
			if (Geometry2D.IsPointInPolygon(clickPosition, collisionShape.Polygon))
			{
				_signals.EmitSignal(nameof(_signals.SkyClicked));
			}
		}
	}

	public void OnIncButtonPressed()
	{
		game_state.Res1 += 10;
		_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
	}

}
