using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using GloryOfRitiria;

public partial class star_system_view : Node2D
{
	private GlobalSignals _signals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		var pallyriaButton = GetNode<TextureButton>("PlanetButton");
		pallyriaButton.Pressed += PallyriaPressed;
		
		var planetButton = GetNode<Button>("Planet2Button");
		// Calling function with parameters using signals
		planetButton.Pressed += () => PlanetButtonPressed(planetButton);
		
		_signals.Connect(nameof(_signals.DetnuraSystemRequested), new Callable(this, nameof(LoadDetnuraSystem)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void PallyriaPressed()
	{
		_signals.EmitSignal(nameof(_signals.PallyriaClicked));
	}

	// Loads planets of Detnura System
	public void LoadDetnuraSystem()
	{
		
	}
	
	private void PlanetButtonPressed(Button button)
	{
		var pallyriaScene = GD.Load<PackedScene>("res://Scenes/Parts/planet_info_window.tscn");
		var inst = (Panel) pallyriaScene.Instantiate();
		
		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]Random Planet[/b]\nMolten Planet";
		
		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load("res://Assets/Img/tmp/MoltenPlanet.png");
		
		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		// On click of exit button destroy the info window and enable button again
		exitButton.Pressed += () => inst.QueueFree(); button.Disabled = false;
		
		// Set the info window in the top right corner
		// Better to do it with H/VBoxes I guess...
		//inst.SetPosition(new Godot.Vector2(GetViewportRect().Size.X - inst.Size.X, 0 + inst.Size.Y));

		AddChild(inst);

		// Disable the planet button so that that player can't open the window multiple times
		// It's probably better to disable more than just the button. Or maybe not.
		button.Disabled = true;
	}
}
