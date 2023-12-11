using Godot;
using System;
using System.Collections.Generic;
using GloryOfRitiria;

public partial class base_game_scene : Node2D
{
	
	private GlobalSignals _signals;
	private event_manager _eventManager;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_eventManager = GetNode<event_manager>("/root/EventManager");
		
		LoadPallyria(); // Pallyria will be the default scene
		
		// Button to open events
		var eventsButton = GetNode<Button>("TestButton");
		eventsButton.Pressed += BuildMultiEventWindow;

		TopBarUpdate();
		

		_signals.Connect(nameof(_signals.SkyClicked), new Callable(this, nameof(LoadDetnuraMap)));
		_signals.Connect(nameof(_signals.PallyriaClicked), new Callable(this, nameof(LoadPallyria)));
		_signals.Connect(nameof(_signals.TurnPassed), new Callable(this, nameof(NewTurn)));
		_signals.Connect(nameof(_signals.TopBarUpdateRequired), new Callable(this, nameof(TopBarUpdate)));
		_signals.Connect(nameof(_signals.WarningWindowRequested), new Callable(this, nameof(BuildWarningWindow)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// Load a scene with a specified path, background path, and optional signal emission
	private async void LoadScene(string scenePath, string backgroundPath, string signal = null)
	{
		ClearScene(); // Clear old scene
		
		// Pause the game for a very short time to make sure that player can't change scene again before old scene
		// is cleared. TODO: Check if 0.05 is still enough on weak PC
		GetTree().Paused = true;
		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		GetTree().Paused = false;

		// Adding the scene to CurrentScene node
		var currentScene = GetNode<Node2D>("CurrentScene");
		var scene = GD.Load<PackedScene>(scenePath);
		var inst = (Node2D)scene.Instantiate();
		currentScene.AddChild(inst);

		// Send a signal if specified
		if (!string.IsNullOrEmpty(signal)) _signals.EmitSignal(signal);

		// Changing BG in base_game_scene
		var bg = GetNode<TextureRect>("BackGroundImage");
		var texture = (Texture2D)GD.Load(backgroundPath);
		bg.Texture = texture;
	}

	
	// Load the Pallyria scene
	public void LoadPallyria()
	{
		LoadScene("res://Scenes/planet_game_scene.tscn", "res://Assets/Img/tmp/PallyriaBG.jpg");
	}
	
	// Load the Detnura scene
	public void LoadDetnuraMap()
	{
		LoadScene("res://Scenes/star_system_view.tscn", "res://Assets/Img/tmp/DetnuraSystemBG.jpg", nameof(_signals.DetnuraSystemRequested));
	}
	
	// Load the Pallyria scene
	public void LoadInterstellarMap()
	{
		ClearScene();
		//TODO
	}

	// Clears loaded scene
	public void  ClearScene()
	{
		var currentScene = GetNode<Node2D>("CurrentScene");
		if (currentScene.GetChildren().Count > 0)
		{
			// Get the scene Node (Pallyria, Interstellar...). It should always be the only child.
			// It's possible to change scene multiple times within a single frame so...
			// Well I guess that's what's happening
			
			currentScene.GetChild<Node2D>(0).QueueFree();
			
		}
	}
	
	public void NewTurn()
	{
		game_state.CurrentTurn += 1;
		game_state.SetCurrentYear();
		game_state.UpdateResources();
		TopBarUpdate();
		UpdateEvents();
		InvokeEvents();
	}

	public void TopBarUpdate()
	{
		var yearLabel = GetNode<Label>("TopBar/CurrentYear");
		yearLabel.Text = game_state.CurrentYear;
		
		// Res 1 update
		var res1 = GetNode<HBoxContainer>("TopBar/Resource");
		var res1Text = res1.GetNode<RichTextLabel>("ResText");
		if (game_state.Res1Rate >= 0)
		{
			res1Text.Text = "" + game_state.Res1 + "\n[color=green]+ "+game_state.Res1Rate+"[/color]";
		}
		else
		{
			res1Text.Text = "" + game_state.Res1 + "\n[color=red]- "+ Math.Abs(game_state.Res1Rate)+"[/color]";
		}
		

	}

	// Called on new turn, update the list of satisfied events
	public void UpdateEvents()
	{
		game_state._eventsForTurn = _eventManager.GetSatisfiedEvents();
	}

	public void BuildMultiEventWindow()
	{
		var multiEventWindow = GD.Load<PackedScene>("res://Scenes/Parts/MultiEventWindow.tscn");
		var multiEventInst = (Panel) multiEventWindow.Instantiate();
		
		AddChild(multiEventInst);
	}
	

	public void InvokeEvents()
	{
		if (game_state._eventsForTurn.Count == 0) return;
		GD.Print(game_state._eventsForTurn[0]);
		BuildMultiEventWindow();
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
		 	GetTree().Paused = false;
			warningWindowInst.QueueFree();
		};
		
		AddChild(warningWindowInst);
		GetTree().Paused = true;
	}
	
	
}
