using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scripts.Scenes;

public partial class planet_game_scene : Node2D
{
	private GlobalSignals _signals;
	private Sprite2D _mapSprite;
	private Vector2 _initialMapScale;
	
	private Sprite2D _skySprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		//_gameState = GetNode<game_state>("/root/GameState");
		
		var phoneButton = GetNode<MenuButton>("PhoneButton");
		phoneButton.GetPopup().Connect("id_pressed", new Callable(this, nameof(_popupProcess)));
		phoneButton.Pressed += () =>  _signals.EmitSignal(nameof(_signals.SimpleButtonClicked));

		_mapSprite = GetNode<Sprite2D>("Map/Sprite2D");
		_initialMapScale = _mapSprite.Scale;
		_mapSprite.SelfModulate = Color.Color8(225, 225, 225);
		
		_skySprite = GetNode<Sprite2D>("Sky/Sprite2D");
		_skySprite.SelfModulate = Color.Color8(225, 225, 225);

		SetupTutorials();
	}


	private void _popupProcess(int id)
	{
		switch (id)
		{
			case 0:
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				_signals.EmitSignal(nameof(_signals.ShipyardsButtonClicked));
				break;
			}
			case 1:
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				_signals.EmitSignal(nameof(_signals.FleetBureauButtonClicked));
				break;
			}
			default: GD.Print("Err"); break;
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (!Input.IsMouseButtonPressed(MouseButton.Left)) return;
		
		var clickPosition = GetGlobalMousePosition();
			
		var skyCollision = GetNode<CollisionPolygon2D>("Sky/SkyCollision");
		if (Geometry2D.IsPointInPolygon(clickPosition, skyCollision.Polygon))
		{
			_signals.EmitSignal(nameof(_signals.SkyClicked));
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
		}
	}
	
	// The signal is connected from the editor, it doesn't uses params.... but it'll have to do for now
	private void _on_map_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (!Input.IsMouseButtonPressed(MouseButton.Left)) return;
		_signals.EmitSignal(nameof(_signals.InterstellarMapRequested));
		_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
	}
	
	private void _on_map_mouse_entered()
	{
		_mapSprite.Scale = _initialMapScale * 1.1f;
		_mapSprite.SelfModulate = Color.Color8(255, 255, 255);
	}


	private void _on_map_mouse_exited()
	{
		_mapSprite.Scale = _initialMapScale;
		_mapSprite.SelfModulate = Color.Color8(225, 225, 225);
	}
	
	private void _on_sky_mouse_entered()
	{
		_skySprite.SelfModulate = Color.Color8(255, 255, 255);
	}


	private void _on_sky_mouse_exited()
	{
		_skySprite.SelfModulate = Color.Color8(225, 225, 225);
	}
	
	
	
	/*Tutorials*/
	private void SetupTutorials()
	{
		var firstWindow = GetNode<PanelContainer>("TutorialWindows/FirstWindow");
		var secondWindow = GetNode<PanelContainer>("TutorialWindows/SecondWindow");
		var thirdWindow = GetNode<PanelContainer>("TutorialWindows/ThirdWindow");
		var forthWindow = GetNode<PanelContainer>("TutorialWindows/ForthWindow");
		
		// Exit buttons
		var firstExit = firstWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		firstExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			game_state.CurrentPallyriaTutorial = -1;
		};
		var secondExit = secondWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		secondExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			secondWindow.Visible = false;
			game_state.CurrentPallyriaTutorial = -1;
		};
		var thirdExit = thirdWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		thirdExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			thirdWindow.Visible = false;
			game_state.CurrentPallyriaTutorial = -1;
		};
		var forthExit = forthWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		forthExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			forthWindow.Visible = false;
			game_state.CurrentPallyriaTutorial = -1;
		};
		
		// Next buttons
		var firstNext = firstWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		firstNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			secondWindow.Visible = true;
			game_state.CurrentPallyriaTutorial = 2;
		};
		var secondNext = secondWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		secondNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			secondWindow.Visible = false;
			thirdWindow.Visible = true;
			game_state.CurrentPallyriaTutorial = 3;
		};
		var thirdNext = thirdWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		thirdNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			thirdWindow.Visible = false;
			forthWindow.Visible = true;
			game_state.CurrentPallyriaTutorial = 4;
		};
		var forthNext = forthWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		forthNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			forthWindow.Visible = false;
			game_state.CurrentPallyriaTutorial = -1;
		};

		switch (game_state.CurrentPallyriaTutorial)
		{
			case 1:
				firstWindow.Visible = true;
				break;
			case 2:
				secondWindow.Visible = true;
				break;
			case 3:
				thirdWindow.Visible = true;
				break;
			case 4:
				forthWindow.Visible = true;
				break;
			default:
				firstWindow.Visible = false;
				secondWindow.Visible = false;
				thirdWindow.Visible = false;
				break;
		}
	}
}



