using Godot;
using System;
using GloryOfRitiria.Scripts.Scenes.Parts;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.Utils;

public partial class EmptyShipSlot : PanelContainer
{
	private Shipyard _shipyard;
	public override void _Ready()
	{
		AddChild(CreateTooltipController());
	}

	public void Init(Shipyard shipyard)
	{
		_shipyard = shipyard;
		
		var shipyardName = GetNode<RichTextLabel>("MCont/VBox/ShipyardName");
		shipyardName.Text = "[img]"+ _shipyard.Location.GetSmallImage() + "[/img]  " + _shipyard.ShipyardName;
	}
	
	private TooltipController CreateTooltipController()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.VisualsText = "Commission a spaceship from the " + _shipyard.ShipyardName;
		// TODO: This way mouse_entered works well but the tooltip is cut at the top
		inst.OwnerPath = new NodePath(GetNode<TextureButton>("MCont/VBox/AddButton").GetPath());
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}
}
