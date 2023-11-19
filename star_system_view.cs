using Godot;
using System;
using GloryOfRitiria;

public partial class star_system_view : Node2D
{
	private GlobalSignals signals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		var planetButton = GetNode<TextureButton>("PlanetButton");
		planetButton.Pressed += PlanetButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void PlanetButtonPressed()
	{
		signals.EmitSignal(nameof(signals.PallyriaClicked));
	}
}
