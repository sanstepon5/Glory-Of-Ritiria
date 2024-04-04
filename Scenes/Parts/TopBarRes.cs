using Godot;
using System;

public partial class TopBarRes : HBoxContainer
{
	[Export] public Texture2D Texture;
	[Export] public string TooltipText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<TextureRect>("ResIcon").Texture = Texture;
		GetNode<Control>("TooltipController").TooltipText = TooltipText;
	}

}
