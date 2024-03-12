using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class CargoSelectWindow : PanelContainer
{
	private GlobalSignals _signals;
	public List<Cargo> InitialCargoList;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");

		var cargoVBox = GetNode<VBoxContainer>("VBox/MarginCont/ScrollCont/VBox");

		foreach (var cargo in game_state.CargoStorage)
		{
			cargoVBox.AddChild(_buildCargoOption(cargo));
		}
	}

	private PanelContainer _buildCargoOption(Cargo cargo)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/CargoOption.tscn");
		var inst = (PanelContainer)scene.Instantiate();
		var image = inst.GetNode<TextureRect>("MarginCont/VBoxCont/Image");

		var cargoName = inst.GetNode<RichTextLabel>("MarginCont/VBoxCont/Name");
		cargoName.Text = "[b]" + cargo.Name;

		image.Texture = (Texture2D)GD.Load(cargo.GetImagePath());
		// TODO: Manage cargo tooltip

		var clickableRectangle = inst.GetNode<ClickableRectangle>("MarginCont/ClickableRectangle");
		clickableRectangle.SignalParameter = cargo.Name;
		
		return inst;
	}
}
