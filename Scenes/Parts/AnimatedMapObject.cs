using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.StarSystem;

public partial class AnimatedMapObject : Control
{
	private GlobalSignals _signals;
	public StarSystemInfo System;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void Init(StarSystemInfo system, GlobalSignals signals)
	{
		_signals = signals;
		System = system;

		// Sprite
		var stars = system.SystemStars;
		var animation = "";
		if (stars.Count == 1)
		{
			animation = stars[0].StarClass switch
			{
				StarClass.YellowDwarf => "yellow",
				StarClass.OrangeDwarf => "orange",
				StarClass.RedDwarf => "red",
				_ => "yellow"
			};
		}
		else
		{	// TODO: Add Binary/Trinary systems
			animation = "orange";
		}
		
		var sprite = GetNode<AnimatedSprite2D>("Sprite");
		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Resources/Animations/InterstellarMap/StarSystemAnimations.tres");
		sprite.Animation = animation;
		sprite.Play();
		sprite.Position = new Vector2(0, 0);

		var spriteSize = sprite.SpriteFrames.GetFrameTexture(animation, 0).GetSize();
		
		// Label
		var label = GetNode<RichTextLabel>("Name");
		label.Text = "[center]" + system.SystemName;
		label.Position = sprite.Position + new Vector2(0, spriteSize.Y);
		if (label.Size.X < spriteSize.X)
		{
			label.SetSize(new Vector2(spriteSize.X, label.Size.Y));
		}
		// Center the sprite
		sprite.Position = (sprite.Position + new Vector2(label.Size.X/2 - spriteSize.X/2, 0));

		// Button
		var button = GetNode<Button>("Button");
		button.Position = sprite.Position;
		button.Size = spriteSize;
		button.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.StarViewRequested), system);
		};
		

		Size = spriteSize;
	}

}
