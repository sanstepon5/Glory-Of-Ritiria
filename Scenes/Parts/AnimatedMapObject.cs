using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.StarSystem;

public partial class AnimatedMapObject : Control
{
	private GlobalSignals _signals;
	public StarSystemInfo System;
	private AnimatedSprite2D _sprite;
	private Vector2 _initialScale;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connect("mouse_entered", new Callable(this, nameof(_OnMouseEntered)));
		// Connect("mouse_exited", new Callable(this, nameof(_OnMouseEntered)));
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
		
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
		_sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Resources/Animations/InterstellarMap/StarSystemAnimations.tres");
		_sprite.Animation = animation;
		_sprite.Play();
		_sprite.Position = new Vector2(0, 0);
		_initialScale = _sprite.Scale;

		var spriteSize = _sprite.SpriteFrames.GetFrameTexture(animation, 0).GetSize();
		
		// Label
		var label = GetNode<RichTextLabel>("Name");
		label.Text = "[center]" + system.SystemName;
		label.Position = _sprite.Position + new Vector2(0, spriteSize.Y);
		if (label.Size.X < spriteSize.X)
		{
			label.SetSize(new Vector2(spriteSize.X, label.Size.Y));
		}
		// Center the sprite
		_sprite.Position = (_sprite.Position + new Vector2(label.Size.X/2 - spriteSize.X/2, 0));

		// Button
		var button = GetNode<Button>("Button");
		button.Position = _sprite.Position;
		button.Size = spriteSize;
		button.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.StarViewRequested), system);
		};
		

		Size = spriteSize;
		
		
		// Add this after initializing the sprite
		var ownershipIndicator = GetNode<TextureRect>("Sprite/OwnershipIndicator");  // Replace with the actual node name
		var shaderMaterial = new ShaderMaterial();
		shaderMaterial.Shader = GD.Load<Shader>("res://Assets/Shaders/star_owner_shader.gdshader");

		// Set color based on ownership
		var ownershipColor = System.claimedBy switch
		{
			StarSystemInfo.SystemOwnership.Ritiria => new Color(0.2f, 0.4f, 1.0f, 0.5f) // Blue, semi-transparent
			,
			StarSystemInfo.SystemOwnership.Earth => new Color(0.2f, 1.0f, 0.2f, 0.5f) // Green
			,
			_ => new Color(0.5f, 0.5f, 0.5f, 0f)
		};

		// Apply to the shader
		shaderMaterial.SetShaderParameter("owner_color", ownershipColor);
		shaderMaterial.SetShaderParameter("radius", 0.5);
		GD.Print($"{system.SystemName}: {shaderMaterial.GetShaderParameter("owner_color")}");
		ownershipIndicator.Material = shaderMaterial;
		// TODO: update on turn change
	}
	
	// Changing the scale is not enough, the sprite must change position with the label as well
	// private void _OnMouseEntered()
	// {
	// 	_sprite.Scale = _initialScale * 3f;
	// }
	//
	//
	// private void _OnMouseExited()
	// {
	// 	_sprite.Scale = _initialScale;
	// }

}
