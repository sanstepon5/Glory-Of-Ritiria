using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.StarSystem;

public partial class CBOnSystemMap : VBoxContainer
{
	private GlobalSignals _signals;
	public CelestialBody Body;
	public Vector2 ImageSize;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
	}

	public void Init(CelestialBody body, bool isStar = false)
	{
		Body = body;
		GetNode<Label>("BodyName").Text = body.Name;
		
		// Setting textures for the button
		var bodyButton = GetNode<TextureButton>("BodyButton");
		bodyButton.TextureNormal = (Texture2D)GD.Load(body.GetImage());
		ImageSize = bodyButton.TextureNormal.GetSize();
						
		if (body.Name == "Pallyria")
		{
			bodyButton.Pressed += () =>
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				CelestialBodyPressed(true);
			};
		}
		else
		{
			bodyButton.Pressed += () =>
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				if (isStar)
					_starPressed();
				else
					CelestialBodyPressed();
			};
		}
	}

	private void CelestialBodyPressed(bool isPallyria = false)
	{
		var planetInfoScene = GD.Load<PackedScene>("res://Scenes/Parts/PlanetInfoWindow.tscn");
		var inst = (PlanetInfoWindow)planetInfoScene.Instantiate();

		inst.Body = Body;

		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]" + Body.Name + "[/b]\n" + Body.GetKnownBodyType();

		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load(Body.GetImage());

		var descLabel = inst.GetNode<RichTextLabel>("MCont/VBox/DescMargin/Description");
		descLabel.Text = Body.GetDescription();

		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
		};

		_setupSendButtons(inst);

		if (isPallyria)
		{
			var toPallyriaMargin = inst.GetNode<MarginContainer>("MCont/VBox/ToPallyriaMargin");
			toPallyriaMargin.Visible = true;
			var toPallyriaButton = toPallyriaMargin.GetNode<Button>("ToPallyriaButton");
			toPallyriaButton.Pressed += () =>
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				_signals.EmitSignal(nameof(_signals.PallyriaClicked));
				_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
			};
		}

		// Add inst to the infoWindow control node in base scene UI canvas node
		_signals.EmitSignal(nameof(_signals.PlanetInfoWindowRequested), inst);

		// Pause the rest of the game while this window is active.
		GetTree().Paused = true;
	}
	
	// TODO: surely it can be refactored a lot with CelestialBodyPressed
	private void _starPressed()
	{
		if (Body is not Star star)
		{
			GD.Print("Tried assigning star methods to a normal body");
			return;
		}
		
		var planetInfoScene = GD.Load<PackedScene>("res://Scenes/Parts/PlanetInfoWindow.tscn");
		var inst = (PlanetInfoWindow)planetInfoScene.Instantiate();

		inst.Body = star;

		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]" + star.Name + "[/b]\n" + star.StarClass;

		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load(star.GetImage());

		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
		};

		_setupSendButtons(inst);
		
		// Add inst to the infoWindow control node in base scene UI canvas node
		_signals.EmitSignal(nameof(_signals.PlanetInfoWindowRequested), inst);

		// Pause the rest of the game while this window is active.
		GetTree().Paused = true;
	}
	
	
	private void _setupSendButtons(PlanetInfoWindow inst)
	{
		var sendButtonMargin = inst.GetNode<MarginContainer>("MCont/VBox/SendShipMargin");
		var sendButton = sendButtonMargin.GetNode<Button>("SendShipButton");
		if (game_state.SelectedShip != null
			&& game_state.SelectedShip.State != ShipState.InRoute
			&& game_state.SelectedShip.Location != Body)
		{
			var missionButtonMargin = inst.GetNode<MarginContainer>("MCont/VBox/MissionMargin");
			
			missionButtonMargin.Visible = true;
			var missionButton = missionButtonMargin.GetNode<OptionButton>("MissionSelect");
			if (game_state.SelectedShip.IsInRouteTo(Body))
			{	// If the ship is already in route here, show the disabled button
				sendButtonMargin.Visible = true;
				missionButton.Disabled = true;
				//missionButton.Selected = 0; Set current mission of the ship
				sendButton.Disabled = true;
				sendButton.Text = game_state.SelectedShip.Name + " in route here";
			}
			else sendButton.Disabled = false;

			missionButton.AddItem("Move ship");
			for (var i = 0; i < game_state.SelectedShip.GetAllMissions().Count; i++)
			{
				var mission = game_state.SelectedShip.GetAllMissions()[i];
				missionButton.AddItem(mission.Name);
				if (!Body.IsMissionCompatible(mission))
					missionButton.SetItemDisabled(i+1, true); // index 0 is for moving ship
			}
			
			missionButton.Selected = -1;

			missionButton.ItemSelected += index => // After player chooses a mission
			{
				sendButtonMargin.Visible = true;
	
				sendButton.Pressed += () =>
				{
					_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
					_handleSendButton(sendButton, missionButton, index);
				};
			};
		}
	}
	
	/**
	 * <param name="body">body that was clicked</param>
	 * <param name="sendButton">Button sendButton: send button node</param>
	 * <param name="missionButton">OptionButton missionButton: select mission button</param>
	 * <param name="index">long index: index of the option selected in missionButton</param>
	 */
	private void _handleSendButton(Button sendButton, OptionButton missionButton, long index)
	{
		game_state.SelectedShip.StartRoute(Body);
		_signals.EmitSignal(nameof(_signals.ShipStartedRoute), game_state.SelectedShip);
		sendButton.Disabled = true;
		sendButton.Text = game_state.SelectedShip.Name + " is in route here";

		if (missionButton.Selected > 0)
		{ // The first one is simple movement which isn't a mission
			game_state.SelectedShip.SetShipMission(missionButton.GetItemText((int)index), Body);
		}
	}

}
