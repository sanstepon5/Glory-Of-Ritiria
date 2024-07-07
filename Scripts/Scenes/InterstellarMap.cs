using Godot;
using System;
using System.Collections.Generic;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using StarSystemInfo = GloryOfRitiria.Scripts.StarSystem.StarSystemInfo;

public partial class InterstellarMap : Node2D
{
	private GlobalSignals _signals;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		//BuildDetnura();
		
		foreach (var starSystemInfo in game_state.DiscoveredSystems)
		{
			BuildSystem(starSystemInfo);
		}

		SetupTutorials();

	}

	public void BuildSystem(StarSystemInfo systemInfo)
	{
		var starSystem = GD.Load<PackedScene>("res://Scenes/Parts/AnimatedMapObject.tscn");
		var starSystemInst = (AnimatedMapObject) starSystem.Instantiate();
		
		starSystemInst.Init(systemInfo, _signals);
		
		// Position the system relative to the center of the map (which is Detnura)
		var centerVBox = GetNode<Node2D>("CirclesCenter");
		starSystemInst.Position = systemInfo.GetPositionOnPlan(centerVBox.Position) ; // + centerVBox.Size/2
		
		
		AddChild(starSystemInst);
	}
	
	
	/*Tutorials*/
	private void SetupTutorials()
	{
		var firstWindow = GetNode<PanelContainer>("CanvasLayer/TutorialWindows/FirstWindow");
		var secondWindow = GetNode<PanelContainer>("CanvasLayer/TutorialWindows/SecondWindow");
		
		// Exit buttons
		var firstExit = firstWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		firstExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			game_state.CurrentInterstellarTutorial = -1;
		};
		var secondExit = secondWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		secondExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			secondWindow.Visible = false;
			game_state.CurrentInterstellarTutorial = -1;
		};
		
		// Next buttons
		var firstNext = firstWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		firstNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			secondWindow.Visible = true;
			game_state.CurrentInterstellarTutorial = 2;
		};
		var secondNext = secondWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		secondNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			secondWindow.Visible = false;
			game_state.CurrentInterstellarTutorial = -1;
		};

		switch (game_state.CurrentInterstellarTutorial)
		{
			case 1:
				firstWindow.Visible = true;
				break;
			case 2:
				secondWindow.Visible = true;
				break;
			default:
				firstWindow.Visible = false;
				secondWindow.Visible = false;
				break;
		}
	}
}
