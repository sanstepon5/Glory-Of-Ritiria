using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Scenes.Parts;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.Utils;

public partial class CargoOption : PanelContainer
{
	private GlobalSignals _signals;
	private Cargo _cargo; 
	
	public override void _Ready()
	{
		AddChild(CreateTooltipController());
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
	
	
	private TooltipController CreateTooltipController()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.MinimumXSize = 300;
		inst.VisualsText = _cargo.GetFullDescription();
		inst.OwnerPath = new NodePath(GetNode<Button>("MarginCont/Button").GetPath());
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}
}
