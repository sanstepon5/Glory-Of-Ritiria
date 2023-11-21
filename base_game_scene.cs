using Godot;
using System;
using GloryOfRitiria;

public partial class base_game_scene : Node2D
{
	
	private GlobalSignals _signals;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		LoadPallyria(); // Pallyria will be the default scene
		

		_signals.Connect(nameof(_signals.SkyClicked), new Callable(this, nameof(LoadDetnuraMap)));
		_signals.Connect(nameof(_signals.PallyriaClicked), new Callable(this, nameof(LoadPallyria)));
		_signals.Connect(nameof(_signals.TurnPassed), new Callable(this, nameof(NewTurn)));
		
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
		var pallyriaScene = GD.Load<PackedScene>("res://planet_game_scene.tscn");
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
		var detnuraScene = GD.Load<PackedScene>("res://star_system_view.tscn");
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
	}

	public void TopBarUpdate()
	{
		var yearLabel = GetNode<Label>("TopBar/CurrentYear");
		yearLabel.Text = game_state.CurrentYear;
	}
}
