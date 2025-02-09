using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

public partial class NextTurnButton : VBoxContainer
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		var button = GetNode<TextureButton>("PassTurnButton");
		button.Pressed	+= () =>
		{
			switch (CheckNextTurnPossible())
			{
				case "ok":
					_signals.EmitSignal(nameof(_signals.TurnPassed));
					UpdateLabels();
					break;
				case "events":
					string message = "Before going to next turn you have to read all events first";
					_signals.EmitSignal(nameof(_signals.WarningWindowRequested), message);
					break;
			}
		};
		
		var tooltipInst = CreateTooltipController();
		button.AddChild(tooltipInst);
	}
	
	
	public string CheckNextTurnPossible()
	{
		if (game_state.EventsForTurn.Count != 0) return "events";
		// Other turn blocking conditions here
		return "ok";

	}
	
	private void UpdateLabels()
	{
		var turnNumber = GetNode<RichTextLabel>("CurrentTurn");
		turnNumber.Text = "[center]Current Turn: " + game_state.CurrentTurn;
	}
	
	private TooltipController CreateTooltipController()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.VisualsText = "Pass week";
		inst.OwnerPath = new NodePath(GetNode<TextureButton>("PassTurnButton").GetPath());
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}

}
