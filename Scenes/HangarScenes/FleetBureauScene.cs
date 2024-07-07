using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

public partial class FleetBureauScene : Node2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.EmitSignal(nameof(_signals.FleetBureauSceneOpened));
		SetupTutorials();
	}
	
	
	/*Tutorials*/
	private void SetupTutorials()
	{
		var firstWindow = GetNode<PanelContainer>("TutorialWindows/FirstWindow");
		
		// Exit buttons
		var firstExit = firstWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		firstExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			game_state.CurrentFleetBureauTutorial = -1;
		};
		
		// Next buttons
		var firstNext = firstWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		firstNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			game_state.CurrentFleetBureauTutorial = -1;
		};

		switch (game_state.CurrentFleetBureauTutorial)
		{
			case 1:
				firstWindow.Visible = true;
				break;
			default:
				firstWindow.Visible = false;
				break;
		}
	}

}
