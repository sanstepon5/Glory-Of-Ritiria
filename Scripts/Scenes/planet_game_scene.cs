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