using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;

public partial class DebugMenu : PanelContainer
{
	private GlobalSignals _signals;
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		var materialButton = GetNode<Button>("MarginCont/VBox/Material10");
		materialButton.Pressed += () => {
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			game_state.AddMaterialRes(10);
			_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
		};
		var scienceButton = GetNode<Button>("MarginCont/VBox/Science10");
		scienceButton.Pressed += () => {
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			game_state.AddScientificRes(10);
			_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
		};
		var politicalButton = GetNode<Button>("MarginCont/VBox/Political10");
		politicalButton.Pressed += () => {
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			game_state.AddPoliticalRes(10);
			_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
		};
		
		var exitButton = GetNode<Button>("MarginCont/VBox/HBox/Exit");
		exitButton.Pressed += () => {
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			Visible = false;
		};
	}
}
