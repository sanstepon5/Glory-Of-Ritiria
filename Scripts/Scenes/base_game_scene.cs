using Godot;
using System;
using System.Collections.Generic;
using GloryOfRitiria;

public partial class base_game_scene : Node2D
{
	
	private GlobalSignals _signals;
	private event_manager _eventManager;
	private List<GameEvent> _eventsForTurn;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_eventManager = GetNode<event_manager>("/root/EventManager");
		_eventsForTurn = new List<GameEvent>();
		
		LoadPallyria(); // Pallyria will be the default scene
		

		_signals.Connect(nameof(_signals.SkyClicked), new Callable(this, nameof(LoadDetnuraMap)));
		_signals.Connect(nameof(_signals.PallyriaClicked), new Callable(this, nameof(LoadPallyria)));
		_signals.Connect(nameof(_signals.TurnPassed), new Callable(this, nameof(NewTurn)));
		_signals.Connect(nameof(_signals.TopBarUpdateRequired), new Callable(this, nameof(TopBarUpdate)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// Load the Pallyria scene
	public void LoadPallyria()
	{
		
		ClearScene(); // Clear old scene
		
		// Adding the Pallyria scene to Current
		var currentScene = GetNode<Node2D>("CurrentScene");
		var pallyriaScene = GD.Load<PackedScene>("res://Scenes/planet_game_scene.tscn");
		var inst = (Node2D) pallyriaScene.Instantiate();

		currentScene.AddChild(inst);
		
		// Changing BG
		var bg = GetNode<TextureRect>("BackGroundImage");
		var texture = (Texture2D)GD.Load("res://Assets/Img/tmp/PallyriaBG.jpg");
		bg.Texture = texture;
	}
	
	// Load the Detnura scene
	public void LoadDetnuraMap()
	{
		ClearScene(); // Clear old scene
		
		// Adding the Detnura scene to Current
		var currentScene = GetNode<Node2D>("CurrentScene");
		var detnuraScene = GD.Load<PackedScene>("res://Scenes/star_system_view.tscn");
		var inst = (Node2D) detnuraScene.Instantiate();
		currentScene.AddChild(inst);
		// The scene is added. Now I should send a signal that will be captured by that scene to load the Detnura system
		_signals.EmitSignal(nameof(_signals.DetnuraSystemRequested));
		
		// Changing BG
		var bg = GetNode<TextureRect>("BackGroundImage");
		var texture = (Texture2D)GD.Load("res://Assets/Img/tmp/DetnuraSystemBG.jpg");
		bg.Texture = texture;
	}
	
	// Load the Pallyria scene
	public void LoadInterstellarMap()
	{
		ClearScene();
		//TODO
	}

	// Clears loaded scene
	public void ClearScene()
	{
		var currentScene = GetNode<Node2D>("CurrentScene");
		if (currentScene.GetChildren().Count > 0)
		{
			// Get the scene Node (Pallyria, Interstellar...). It should always be the only child.
			currentScene.GetChild<Node2D>(0).QueueFree();
		}
		
		
	}
	
	public void NewTurn()
	{
		GD.Print("game_state.CurrentTurn");
		GD.Print(game_state.CurrentTurn);
		game_state.CurrentTurn += 1;
		game_state.SetCurrentYear();
		TopBarUpdate();
		UpdateEvents();
		InvokeEvents();
	}

	public void TopBarUpdate()
	{
		var yearLabel = GetNode<Label>("TopBar/CurrentYear");
		yearLabel.Text = game_state.CurrentYear;
		
		var res1 = GetNode<HBoxContainer>("TopBar/Resource");
		var res1Text = res1.GetNode<RichTextLabel>("ResText");
		res1Text.Text = "" + game_state.Res1;

	}

	// Called on new turn, update the list of satisfied events
	public void UpdateEvents()
	{
		_eventsForTurn = _eventManager.GetSatisfiedEvents();
	}

	public Panel BuildEventWindow(GameEvent e)
	{
		var eventWindow = GD.Load<PackedScene>("res://Scenes/Parts/event_window.tscn");
		var inst = (Panel) eventWindow.Instantiate();
		
		var title = inst.GetNode<RichTextLabel>("MBox/VBox/TitleHBox/TitleLabel");
		title.Text = e.Name;
		
		var image = inst.GetNode<TextureRect>("MBox/VBox/ImageMBox/EventImage");
		image.Texture = (Texture2D)GD.Load(e.ImagePath);
		
		var desc = inst.GetNode<RichTextLabel>("MBox/VBox/DescMBox/DescLabel");
		desc.Text = e.Description;
		
		var exitOption = inst.GetNode<Button>("MBox/VBox/OptionsMBox/OptionsVBox/DefaultButton");
		exitOption.Pressed += () => inst.QueueFree();

		return inst;
	}

	public Panel BuildMultiEventWindow()
	{
		// var multiEventWindow = GD.Load<PackedScene>("res://Scenes/Parts/MultiEventWindow.tscn");
		// var multiEventInst = (Panel) multiEventWindow.Instantiate();
		// var leftEventButton = multiEventInst.GetNode<Button>("VBox/HBoxEventHandling/MBoxLeft");
		// leftEventButton.Pressed += ;
		return null;
	}

	public void InvokeEvents()
	{
		if (_eventsForTurn.Count == 0) return;
		if (_eventsForTurn.Count == 1)
		{
			var gameEvent = _eventsForTurn[0];
			GD.Print(gameEvent.Id);
			AddChild(BuildEventWindow(gameEvent));
		}
		else
		{
			
		}
	}
}
