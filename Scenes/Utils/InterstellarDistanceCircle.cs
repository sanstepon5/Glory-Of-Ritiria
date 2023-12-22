using System;
using Godot;

namespace GloryOfRitiria.Scenes.Utils;

public partial class InterstellarDistanceCircle : Node2D
{
	[Export(PropertyHint.Range, "0, 3000, 0.5")]  public float Radius = 250;
	[Export(PropertyHint.Range, "0, 5, 0.5")] public float Thickness = 3;
	[Export] public string DistanceText = "5 Light Years";
	[Export] public Color Color = Colors.Blue;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var label = GetNode<Label>("DistanceLabel");
		label.Text = DistanceText;
		label.Position += new Vector2(0, -Radius);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Draw()
	{
		// Point count can be optimized based on current resolution
		DrawArc(new Vector2(0, 0), Radius, 0, (float)(2 * Math.PI), 3000, Color, Thickness, true);
	}
}
