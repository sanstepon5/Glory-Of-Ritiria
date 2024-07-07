using Godot;
using System;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.Utils;

public partial class ProgressShipSlot : PanelContainer
{
	private Shipyard _shipyard;
	
	public override void _Ready()
	{
		// TODO: Find a way to add a tooltip without a button
		// AddChild(CreateTooltipController());
	}

	
	public void Init(Shipyard shipyard)
	{
		_shipyard = shipyard;
		
		var shipName = GetNode<RichTextLabel>("MCont/VBox/TopHBox/NameLabel");
		shipName.Text = _shipyard.Ship.Name;
		
		var shipyardName = GetNode<RichTextLabel>("MCont/VBox/ShipyardName");
		shipyardName.Text = "[img]"+ _shipyard.Location.GetSmallImage() + "[/img]  " + _shipyard.ShipyardName;
		
		var progressBar = GetNode<ProgressBar>("MCont/VBox/CenterVBox/MCont/ProgressBar");
		var progressValue =  _shipyard.CurrentProgress / _shipyard.TurnCost * 100;
		progressBar.Value = progressValue;
		
		var efficiencyLabel = GetNode<RichTextLabel>("MCont/VBox/CenterVBox/EfficiencyLabel");
		efficiencyLabel.Text = "Current Efficiency : " + Math.Round(_shipyard.BuildingSpeed*100, 4) + "%";
		
		var turnsLeftLabel = GetNode<RichTextLabel>("MCont/VBox/CenterVBox/RichTextLabel");
		var turnsLeft = Math.Ceiling((_shipyard.TurnCost - _shipyard.CurrentProgress) / _shipyard.BuildingSpeed);
		if (turnsLeft > 1)
			turnsLeftLabel.Text = turnsLeft + " turns left";
		else 
			turnsLeftLabel.Text = "Completed on next turn";
	}
	
	// private TooltipController CreateTooltipController()
	// {
	// 	var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
	// 	var inst = (TooltipController)scene.Instantiate();
	//
	// 	inst.Name = "TooltipController";
	// 	inst.VisualsText = $"The {_shipyard.ShipyardName} is currently building";
	// 	inst.OwnerPath = new NodePath(GetPath());
	// 	inst.EnterDelay = 0.3;
	// 	inst.ExitDelay = 0.3;
	//
	// 	return inst;
	// }
}
