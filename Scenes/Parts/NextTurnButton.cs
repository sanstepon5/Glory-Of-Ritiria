using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;

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

}
