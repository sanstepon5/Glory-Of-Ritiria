using Godot;
using GloryOfRitiria.Scripts.Utils;

public partial class EventChoice : Button
{
	public string TextOnTooltip = "";

	public override void _Ready()
	{
		var tooltipInst = CreateTooltipController();
		AddChild(tooltipInst);
	}
	
	private TooltipController CreateTooltipController()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.MinimumXSize = 300;
		inst.VisualsText = TextOnTooltip;
		inst.OwnerPath = new NodePath(GetPath());
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}


}
