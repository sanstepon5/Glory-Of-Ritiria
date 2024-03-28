using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

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
		var eventsButton = GetNode<Button>("UICanvas/TopBar/TestButton");
		eventsButton.Pressed += BuildMultiEventWindow;
		
		var interStellarMapButton = GetNode<Button>("UICanvas/TopBar/InterstellarMapButton");
		interStellarMapButton.Pressed += LoadInterstellarMap;

		TopBarUpdate();

		_signals.Connect(nameof(_signals.SkyClicked), new Callable(this, nameof(LoadDetnuraMap)));
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
		
		_signals.Connect(nameof(_signals.SimpleButtonClicked), new Callable(this, nameof(_playSimplePlayButtonSound)));
		_signals.Connect(nameof(_signals.NextEventButtonClicked), new Callable(this, nameof(_playNextEventButtonSound)));
		_signals.Connect(nameof(_signals.EventOptionButtonClicked), new Callable(this, nameof(_playEventOptionButtonSound)));
	}

	
	// Load a scene with a specified path, background path, and optional signal emission
	// TODO: refactor, too many different scenes to load
	private async void LoadScene(string scenePath, string backgroundPath, string signal = null, StarSystemInfo systemInfo=null, params String[] signalArgs)
	{
		ClearScene(); // Clear old scene
		
		// Pause the game for a very short time to make sure that player can't change scene again before old scene
		// is cleared.
		GetTree().Paused = true;
		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		GetTree().Paused = false;

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
		var bg = GetNode<TextureRect>("BackGroundImage");
		var texture = (Texture2D)GD.Load(backgroundPath);
		bg.Texture = texture;
		
		// By default all scenes show the pass turn button, if needed should be disabled in scene load function
		//_signals.EmitSignal(nameof(_signals.ShowPassTurnButtonRequested));
	}

	
	// Load the Pallyria scene
	public void LoadPallyria()
	{
		LoadScene("res://Scenes/planet_game_scene.tscn", "res://Assets/Img/tmp/PallyriaOffice.png");
	}
	
	public void LoadShipyards()
	{
		LoadScene("res://Scenes/HangarScenes/ShipyardsScene.tscn", "res://Assets/Img/tmp/hangar.jpg");
	}
	
	public void LoadFleetBureau()
	{
		LoadScene("res://Scenes/HangarScenes/FleetBureauScene.tscn", "res://Assets/Img/tmp/hangar.jpg");
	}
	
	// Load the Detnura scene
	public void LoadDetnuraMap()
	{
		LoadScene("res://Scenes/star_system_view.tscn", "res://Assets/Img/tmp/SystemBackGround.png", nameof(_signals.DetnuraBuildRequested));
	}	
	
	// Load a star system view scene
	public void LoadSystemMap(StarSystemInfo systemInfo)
	{
		LoadScene("res://Scenes/star_system_view.tscn", "res://Assets/Img/tmp/SystemBackGround.png", nameof(_signals.StarViewBuildRequested), systemInfo);
	}
	
	// Load the Pallyria scene
	public void LoadInterstellarMap()
	{
		// TODO: Find a background and generally better integrate this scene in the current framework
		LoadScene("res://Scenes/InterstellarMap.tscn", "res://Assets/Img/tmp/GalaxyBackGround.png");
	}

	// Clears loaded scene
	public void  ClearScene()
	{
		//_signals.EmitSignal(nameof(_signals.ShowPassTurnButtonRequested));
		var currentScene = GetNode<Node2D>("CurrentScene");
		if (currentScene.GetChildren().Count > 0)
		{
			currentScene.GetChild<Node2D>(0).QueueFree();
		}
	}
	
	public void NewTurn()
	{
		game_state.CurrentTurn += 1;
		game_state.SetCurrentYear();
		game_state.UpdateResources();
		game_state.UpdateShipConstruction();
		game_state.UpdateActiveShips();
		TopBarUpdate();
		// Maybe it would make sense to merge the two
		UpdateEvents();
		InvokeEvents();
	}

	//TODO: Move all that to the TopBar Scene
	public void TopBarUpdate()
	{
		var topBar = GetNode<Panel>("UICanvas/TopBar");
		var yearLabel = topBar.GetNode<Label>("CurrentYear");
		yearLabel.Text = game_state.CurrentYear;
		
		// Res 1 update
		var res1 = topBar.GetNode<HBoxContainer>("ResourceContainer/Res1");
		var res1Text = res1.GetNode<RichTextLabel>("ResText");
		if (game_state.Res1Rate >= 0)
			res1Text.Text = "" + game_state.Res1 + "[color=green] + "+ game_state.Res1Rate+"[/color]";
		else
			res1Text.Text = "" + game_state.Res1 + "\n[color=red] - "+ Math.Abs(game_state.Res1Rate)+"[/color]";
		
		// Science update
		var scientific = topBar.GetNode<HBoxContainer>("ResourceContainer/ScienceRes");
		var scientificText = scientific.GetNode<RichTextLabel>("ResText");
		if 
			(game_state.ScientificRes > 5) scientificText.Text = "" + game_state.ScientificRes + "%";
		else 
			scientificText.Text = "[color=red]" + game_state.ScientificRes + "%[/color]";
		
		// Political power update
		var political = topBar.GetNode<HBoxContainer>("ResourceContainer/PoliticalRes");
		var politicalText = political.GetNode<RichTextLabel>("ResText");
		if (game_state.PoliticalRes > 5) 
			politicalText.Text = "" + game_state.PoliticalRes + "%";
		else 
			politicalText.Text = "[color=red]" + game_state.PoliticalRes + "%[/color]";
		
		// Selected Ship update
		var shipName = topBar.GetNode<RichTextLabel>("SelectedShip/VBox/MarginCont/HBox/ShipName");
		shipName.Text = "No ship selected";
		var planetName = topBar.GetNode<RichTextLabel>("SelectedShip/VBox/MarginCont2/HBox/PlanetName");
		planetName.Text = "...";
		if (game_state.SelectedShip != null)
		{
			shipName.Text = game_state.SelectedShip.Name;
			planetName.Text = game_state.SelectedShip.Location.Name;
		}
	}

	// Called on new turn, update the list of satisfied events
	private void UpdateEvents()
	{
		game_state.EventsForTurn = _eventManager.GetSatisfiedEvents();
	}

	private void InvokeEvents()
	{
		if (game_state.EventsForTurn.Count == 0) return;
		BuildMultiEventWindow();
	}

	private void BuildMultiEventWindow()
	{
		var multiEventWindow = GD.Load<PackedScene>("res://Scenes/Parts/MultiEventWindow.tscn");
		var multiEventInst = (Panel) multiEventWindow.Instantiate();
		
		_eventContainer.Visible = true;
		_eventContainer.AddChild(multiEventInst);
	}	
	
	public void AddPlanetInfoWindow(PanelContainer window)
	{
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
			_warningWindowContainer.Visible = false;
			GetTree().Paused = false;
			_warningWindowContainer.GetChild(0).QueueFree();
		};

		_warningWindowContainer.Visible = true;
		_warningWindowContainer.AddChild(warningWindowInst);
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
	}
