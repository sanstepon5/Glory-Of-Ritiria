using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Scenes.Parts;
using StarSystemInfo = GloryOfRitiria.Scripts.StarSystem.StarSystemInfo;

public partial class base_game_scene : Node2D
{
	
	private GlobalSignals _signals;
	private event_manager _eventManager;
	private MarginContainer _eventContainer;
	private MarginContainer _infoWindowContainer;
	private MarginContainer _warningWindowContainer;
	
	/*Sounds*/
	private AudioStreamPlayer _simpleButtonSound;
	private AudioStreamPlayer _nextEventClick;
	private AudioStreamPlayer _eventOptionClick;
	private AudioStreamPlayer _newEventSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Global nodes
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_eventManager = GetNode<event_manager>("/root/EventManager");
		
		// UI nodes
		_eventContainer = GetNode<MarginContainer>("UICanvas/EventWindowControl");
		_infoWindowContainer = GetNode<MarginContainer>("UICanvas/InfoWindowControl");
		_warningWindowContainer = GetNode<MarginContainer>("UICanvas/WarningWindowControl");
		
		// Pallyria will be the default scene
		LoadPallyria(); 
		
		// Button to open events
		var eventsButton = GetNode<Button>("UICanvas/TopBar/EventsButton");
		eventsButton.Pressed += () =>
		{
			_playSimplePlayButtonSound();
			BuildMultiEventWindow();
		};
		
		var interStellarMapButton = GetNode<Button>("UICanvas/TopBar/InterstellarMapButton");
		interStellarMapButton.Pressed += () => {
			_playSimplePlayButtonSound();
			LoadInterstellarMap();
		};
		
		var backToPallyriaButton = GetNode<Button>("UICanvas/TopBar/BackToPallyriaButton");
		backToPallyriaButton.Pressed += () => {
			_playSimplePlayButtonSound();
			LoadPallyria();
		};
				
		TopBarUpdate();
		

		_signals.Connect(nameof(_signals.SkyClicked), new Callable(this, nameof(LoadDetnuraMap)));
		_signals.Connect(nameof(_signals.InterstellarMapRequested), new Callable(this, nameof(LoadInterstellarMap)));
		_signals.Connect(nameof(_signals.ShipyardsButtonClicked), new Callable(this, nameof(LoadShipyards)));
		_signals.Connect(nameof(_signals.FleetBureauButtonClicked), new Callable(this, nameof(LoadFleetBureau)));
		_signals.Connect(nameof(_signals.DetnuraSystemRequested), new Callable(this, nameof(LoadDetnuraMap)));
		
		_signals.Connect(nameof(_signals.StarViewRequested), new Callable(this, nameof(LoadSystemMap)));
		
		_signals.Connect(nameof(_signals.PallyriaClicked), new Callable(this, nameof(LoadPallyria)));
		_signals.Connect(nameof(_signals.TurnPassed), new Callable(this, nameof(NewTurn)));
		_signals.Connect(nameof(_signals.TopBarUpdateRequired), new Callable(this, nameof(TopBarUpdate)));
		_signals.Connect(nameof(_signals.WarningWindowRequested), new Callable(this, nameof(BuildWarningWindow)));
		
		_signals.Connect(nameof(_signals.PlanetInfoWindowRequested), new Callable(this, nameof(AddPlanetInfoWindow)));
		
		_signals.EventWindowClosed += () =>
		{
			// Disable event container when the event window is closed
			_eventContainer.Visible = false;
			GetTree().Paused = false;
			_eventContainer.GetChild(0).QueueFree();
		};
		
		_signals.InfoWindowClosed += () =>
		{
			// Disable event container when the event window is closed
			_infoWindowContainer.Visible = false;
			GetTree().Paused = false;
			_infoWindowContainer.GetChild(0).QueueFree();
		};
		
		
		/*Sounds*/
		_simpleButtonSound = GetNode<AudioStreamPlayer>("Sound/SimpleButtonClick");
		_nextEventClick = GetNode<AudioStreamPlayer>("Sound/NextEventClick");
		_eventOptionClick = GetNode<AudioStreamPlayer>("Sound/EventOptionClick");
		_newEventSound = GetNode<AudioStreamPlayer>("Sound/NewEventSound");
		
		_signals.Connect(nameof(_signals.SimpleButtonClicked), new Callable(this, nameof(_playSimplePlayButtonSound)));
		_signals.Connect(nameof(_signals.NextEventButtonClicked), new Callable(this, nameof(_playNextEventButtonSound)));
		_signals.Connect(nameof(_signals.EventOptionButtonClicked), new Callable(this, nameof(_playEventOptionButtonSound)));
		
		/*Show events on the start of the game*/
		UpdateEvents();
		if (game_state.EventsForTurn.Count > 0)
		{
			// Have to use a timer here because without waiting the window doesn't handle pause properly
			var eventWindowTimer = new Timer();
			eventWindowTimer.OneShot = true;
			eventWindowTimer.Connect("timeout", new Callable(this, nameof(BuildMultiEventWindow)));
			AddChild(eventWindowTimer);
			eventWindowTimer.Start(0.1f);
		}
	}

	
	// Load a scene with a specified path, background path, and optional signal emission
	// TODO: refactor, too many different scenes to load
	private async void LoadScene(string sceneName, string backgroundPath, string signal = null, StarSystemInfo systemInfo=null, params String[] signalArgs)
	{
		// Pause the game for a very short time to make sure that player can't change scene again before old scene
		// is cleared.
		GetTree().Paused = true;
		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		GetTree().Paused = false;
		
		ClearScene(); // Clear old scene

		var scenePath = GetScenePath(sceneName);
		
		// Adding the scene to CurrentScene node
		var currentScene = GetNode<Node2D>("CurrentScene");
		var scene = GD.Load<PackedScene>(scenePath);
		var inst = (Node2D)scene.Instantiate();
		currentScene.AddChild(inst);

		// Send a signal if specified. Can be necessary for initializing scenes (signals to build connect in Ready)
		// Far from good solution for signals with arguments
		if (!string.IsNullOrEmpty(signal))
		{
			if (!signalArgs.IsEmpty())
			{
				_signals.EmitSignal(signal, signalArgs[0]);
			}
			else if (systemInfo != null)
			{
				_signals.EmitSignal(signal, systemInfo);
			}
			else _signals.EmitSignal(signal);
		}

		// Changing BG in base_game_scene
		// TODO: Handle this better
		var bg = GetNode<Sprite2D>("BackGroundImage");
		var staticBg = GetNode<TextureRect>("BG");
		
		if (backgroundPath == "res://Assets/Img/tmp/MilkyWayTransparent.png")
		{
			var texture = GD.Load<Texture2D>(backgroundPath);
			bg.Texture = texture;
			bg.Visible = true;
			staticBg.Visible = false;
			bg.Centered = true;
			bg.Position = GetViewportRect().Size / 2;
		}
		else if (backgroundPath != "")
		{
			var texture = GD.Load<Texture2D>(backgroundPath);
			staticBg.Texture = texture;
			staticBg.Visible = true;
			bg.Visible = false;
		}
		else
		{
			bg.Visible = false;
			staticBg.Visible = false;
		}
	}

	private static string GetScenePath(string sceneName)
	{
		return sceneName switch
		{
			"Pallyria" => "res://Scenes/planet_game_scene.tscn",
			"Shipyards" => "res://Scenes/HangarScenes/ShipyardsScene.tscn",
			"FleetBureau" => "res://Scenes/HangarScenes/FleetBureauScene.tscn",
			"Detnura" => "res://Scenes/star_system_view.tscn",
			"System" => "res://Scenes/star_system_view.tscn",
			"Interstellar" => "res://Scenes/InterstellarMap.tscn",
			_ => ""
		};
	}

	
	// Load the Pallyria scene
	public void LoadPallyria()
	{
		LoadScene("Pallyria", "res://Assets/Img/tmp/PallyriaOffice.png");
	}
	
	public void LoadShipyards()
	{
		LoadScene("Shipyards", "res://Assets/Img/tmp/hangar.jpg");
	}
	
	public void LoadFleetBureau()
	{
		LoadScene("FleetBureau", "res://Assets/Img/tmp/hangar.jpg");
	}
	
	// Load the Detnura scene
	public void LoadDetnuraMap()
	{
		LoadScene("Detnura", "res://Assets/Img/tmp/MilkyWayTransparent.png", nameof(_signals.DetnuraBuildRequested));
	}	
	
	// Load a star system view scene
	public void LoadSystemMap(StarSystemInfo systemInfo)
	{
		LoadScene("System", "res://Assets/Img/tmp/MilkyWayTransparent.png", nameof(_signals.StarViewBuildRequested), systemInfo);
	}
	
	// Load the Pallyria scene
	public void LoadInterstellarMap()
	{
		// TODO: Find a background and generally better integrate this scene in the current framework
		LoadScene("Interstellar", "");
	}

	// Clears loaded scene
	public void  ClearScene()
	{
		var scene = GetNode<Node2D>("CurrentScene");
		if (scene.GetChildren().Count > 0)
		{
			scene.GetChild<Node2D>(0).QueueFree();
		}
	}
	
	public void NewTurn()
	{
		game_state.CurrentTurn += 1;
		game_state.UpdateResources();
		game_state.UpdateShipConstruction();
		game_state.UpdateActiveShips();
		TopBarUpdate();
		// Maybe it would make sense to merge the two
		UpdateEvents();
		InvokeEvents();
	}

	public void TopBarUpdate()
	{
		var topBar = GetNode<GloryOfRitiria.Scenes.Parts.TopBar>("UICanvas/TopBar");
		topBar.Update();
	}

	// Called on new turn, update the list of satisfied events
	private void UpdateEvents()
	{
		game_state.EventsForTurn = _eventManager.GetSatisfiedEvents();
	}

	private void InvokeEvents()
	{
		if (game_state.EventsForTurn.Count == 0) return;
		_playNewEventSound();
		BuildMultiEventWindow();
	}

	private void BuildMultiEventWindow()
	{
		var multiEventWindow = GD.Load<PackedScene>("res://Scenes/Parts/MultiEventWindow.tscn");
		var multiEventInst = (MultiEventWindow) multiEventWindow.Instantiate();
		
		_eventContainer.Visible = true;
		_eventContainer.AddChild(multiEventInst);
	}	
	
	public void AddPlanetInfoWindow(PanelContainer window)
	{
		_playNextEventButtonSound(); //TODO: Replace by unique sound for when window opens
		_infoWindowContainer.Visible = true;
		_infoWindowContainer.AddChild(window);
	}
	
	public void BuildWarningWindow(string message)
	{
		var warningWindow = GD.Load<PackedScene>("res://Scenes/Parts/WarningWindow.tscn");
		var warningWindowInst = (Panel) warningWindow.Instantiate();
		
		var messageLabel = warningWindowInst.GetNode<RichTextLabel>("VBox/MessageMargin/Message");
		messageLabel.Text = message;
		var exitButton = warningWindowInst.GetNode<Button>("VBox/ButtonMargin/OkButton");
		exitButton.Pressed += () =>
		{
			_playSimplePlayButtonSound();
			_warningWindowContainer.Visible = false;
			GetTree().Paused = false;
			_warningWindowContainer.GetChild(0).QueueFree();
		};

		_warningWindowContainer.Visible = true;
		_warningWindowContainer.AddChild(warningWindowInst);
		_playNextEventButtonSound(); //TODO: Replace by unique sound for when window opens
		GetTree().Paused = true;
	}


	
	/*Play sounds*/
	private void _playSimplePlayButtonSound()
	{
		_simpleButtonSound.Play();
	}
	private void _playNextEventButtonSound()
	{
		_nextEventClick.Play();
	}
	private void _playEventOptionButtonSound()
	{
		_eventOptionClick.Play();
	}
	
	private void _playNewEventSound()
	{
		_newEventSound.Play();
	}
	}
