using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.ShipRelated;

public partial class CargoOption : PanelContainer
{
	private GlobalSignals _signals;
	private Cargo _cargo; 
	
	public override void _Ready()
	{
	}

	public void Init(Cargo cargo, GlobalSignals signals)
	{
		_cargo = cargo;
		_signals = signals;
		
		var image = GetNode<TextureRect>("MarginCont/VBoxCont/Image");

		var cargoNameLabel = GetNode<RichTextLabel>("MarginCont/VBoxCont/Name");
		cargoNameLabel.Text = "[b]" + _cargo.Name;

		image.Texture = (Texture2D)GD.Load(_cargo.GetImagePath());
		// TODO: Manage cargo tooltip

		var button = GetNode<Button>("MarginCont/Button");
		button.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.CargoSelectedForOutfit), _cargo.Name);
			_signals.EmitSignal(nameof(_signals.OutfitWindowUpdateRequired));
		};
	}
}
