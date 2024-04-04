using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.ShipRelated;

public partial class CargoOption : PanelContainer
{
	public GlobalSignals Signals;
	public Cargo Cargo; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
	}

	public void Init()
	{
		var image = GetNode<TextureRect>("MarginCont/VBoxCont/Image");

		var cargoNameLabel = GetNode<RichTextLabel>("MarginCont/VBoxCont/Name");
		cargoNameLabel.Text = "[b]" + Cargo.Name;

		image.Texture = (Texture2D)GD.Load(Cargo.GetImagePath());
		// TODO: Manage cargo tooltip

		var button = GetNode<Button>("MarginCont/Button");
		button.Pressed += () =>
		{
			Signals.EmitSignal(nameof(Signals.SimpleButtonClicked));
			Signals.EmitSignal(nameof(Signals.CargoSelectedForOutfit), Cargo.Name);
		};
	}
}
