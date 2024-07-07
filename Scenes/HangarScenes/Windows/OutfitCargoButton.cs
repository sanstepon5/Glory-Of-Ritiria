using Godot;
using System;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.Utils;

// This whole class is messy, I should separate this in two different scenes
public partial class OutfitCargoButton : MarginContainer
{
	private Cargo _cargo;
	private ExtTextureButton _button;

	public override void _Ready()
	{
		if (_cargo is not null)
			AddChild(BuildFullCargoTooltip());
		else
			AddChild(BuildAddCargoTooltip());
	}

	public void InitFullButton(Cargo cargo, bool visible)
	{
		_cargo = cargo;
		_button = GetNode<ExtTextureButton>("PanelCont/ExtTextureButton");

		_button.TextureNormal = (Texture2D)GD.Load(cargo.GetImagePath());
		_button.TextureHover = (Texture2D)GD.Load("res://Assets/Icons/change.png");

		GetNode<TextureRect>("NewlyAddedImage").Visible = visible;
	}
	
	public void InitAddButton()
	{
		_cargo = null;
		_button = GetNode<ExtTextureButton>("PanelCont/ExtTextureButton");

		_button.TextureNormal = (Texture2D)GD.Load("res://Assets/Icons/empty.png");
		_button.TextureHover = (Texture2D)GD.Load("res://Assets/Icons/plus.png");
	}
	
	private TooltipController BuildFullCargoTooltip()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.MinimumXSize = 300;
		inst.VisualsText =
			$"[img]res://Assets/Icons/leftMouseClick32.png[/img] to replace {_cargo.Name}, " +
			$"[img]res://Assets/Icons/rightMouseClick32.png[/img] to remove it";
		// TODO: This way mouse_entered works well but the tooltip is cut at the top
		inst.OwnerPath = _button.GetPath();
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}
	
	private TooltipController BuildAddCargoTooltip()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.MinimumXSize = 300;
		inst.VisualsText =
			$"[img]res://Assets/Icons/leftMouseClick32.png[/img] to add a new module";
		// TODO: This way mouse_entered works well but the tooltip is cut at the top
		inst.OwnerPath = _button.GetPath();
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}
}
