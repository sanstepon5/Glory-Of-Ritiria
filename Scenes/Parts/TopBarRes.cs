using Godot;
using GloryOfRitiria.Scripts.Utils;

public partial class TopBarRes : HBoxContainer
{
	[Export] public Texture2D Texture;
	[Export] public string TextOnTooltip;
	// TODO: Add resource income in tooltip. This might require refactoring the entire res get set system and adding signals...
	public override void _Ready()
	{
		GetNode<TextureRect>("ResIcon").Texture = Texture;
		
		var tooltipInst = CreateTooltipController();
		AddChild(tooltipInst);
	}

	private TooltipController CreateTooltipController()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.VisualsText = TextOnTooltip;
		inst.OwnerPath = new NodePath(GetPath());
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}

}
