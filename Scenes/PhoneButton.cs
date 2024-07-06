using Godot;
using System;

public partial class PhoneButton : MenuButton
{
	private Vector2 _initialScale;
	private Sprite2D _sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("mouse_entered", new Callable(this, nameof(_mouseEntered)));
		Connect("mouse_exited", new Callable(this, nameof(_mouseExited)));
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_initialScale = _sprite.Scale;
		_sprite.SelfModulate = Color.Color8(225, 225, 225);
	}


	private void _mouseEntered()
	{
		_sprite.Scale = _initialScale * 1.1f;
		_sprite.SelfModulate = Color.Color8(255, 255, 255);
		
	}
	private void _mouseExited()
	{
		_sprite.Scale = _initialScale;
		_sprite.SelfModulate = Color.Color8(225, 225, 225);
	}
}
