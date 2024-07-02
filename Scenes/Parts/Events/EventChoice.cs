using GloryOfRitiria.Scripts.Scenes.Parts;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scenes.Parts.Events;

public partial class EventChoice : Button
{
	public Choice Choice;

	public override void _Ready()
	{
		var tooltipInst = CreateTooltipController();
		AddChild(tooltipInst);
	}

	public void Init(Choice choice)
	{
		Choice = choice;
		Text = choice.Desc;
		// The click effects will have to be assigned in parent node for now
	}
	
	private TooltipController CreateTooltipController()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.MinimumXSize = 300;
		inst.VisualsText = Choice.GetFullTooltipText();
		inst.OwnerPath = new NodePath(GetPath());
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}


}
