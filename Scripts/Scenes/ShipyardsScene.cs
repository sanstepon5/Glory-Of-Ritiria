using GloryOfRitiria.Scenes.HangarScenes.Windows;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using GloryOfRitiria.Scripts.ShipRelated;
using Shipyard = GloryOfRitiria.Scripts.ShipRelated.Shipyard;

namespace GloryOfRitiria.Scripts.Scenes;

public partial class ShipyardsScene : Node2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.Connect(nameof(_signals.ConstructionWindowRequested), new Callable(this, nameof(BuildConstructionWindow)));
		
		_signals.Connect(nameof(_signals.FullSlotClicked), new Callable(this, nameof(BuildOutfittingWindow)));
		_signals.Connect(nameof(_signals.AddCargoClicked), new Callable(this, nameof(BuildCargoSelectWindow)));
		
		
		_signals.Connect(nameof(_signals.ThirdLevelProcessEntered), new Callable(this, nameof(DisableRightWindowProcess)));
		_signals.Connect(nameof(_signals.ThirdLevelProcessExited), new Callable(this, nameof(EnableRightWindowProcess)));
		

		InitBodiesSelectionMenu();
		_signals.EmitSignal(nameof(_signals.ShipyardsSceneOpened));
		SetupTutorials();
	}

	public void BuildConstructionWindow(Shipyard shipyard)
	{
		var windowCont = GetNode<MarginContainer>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/ShipConstructionWindow.tscn");
		var inst = (ShipConstructionWindow)scene.Instantiate();
		inst.Init(shipyard, _signals);
		
		windowCont.AddChild(inst);
	}	
	
	public void BuildOutfittingWindow(Ship ship)
	{
		var windowCont = GetNode<MarginContainer>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/ShipOutfittingWindow.tscn");
		var inst = (ShipOutfittingWindow)scene.Instantiate();
		inst.Init(ship, _signals);
		
		windowCont.AddChild(inst);
	}
	
	public void BuildCargoSelectWindow()
	{
		var windowCont = GetNode<MarginContainer>("CenterWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/CargoSelectWindow.tscn");
		var inst = (CargoSelectWindow)scene.Instantiate();
		inst.Init(_signals);
		
		windowCont.AddChild(inst);
	}


	private void InitBodiesSelectionMenu()
	{
		// I shouldn't access a scene children here...
		var bodiesHBox = GetNode<HBoxContainer>("ShipyardSelectionMenu/HBoxContainer");
		foreach (var body in game_state.BodiesWithShipyards)
		{
			var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/HangarPlanetSelectionScene.tscn");
			var inst = (HangarPlanetSelectionScene)scene.Instantiate();
			inst.Init(body, _signals);
			bodiesHBox.AddChild(inst);
		}
	}

	public void DisableRightWindowProcess()
	{
		var windowCont = GetNode<MarginContainer>("RightWindow");
		windowCont.ProcessMode = ProcessModeEnum.Disabled;
	}
	public void EnableRightWindowProcess()
	{
		var windowCont = GetNode<MarginContainer>("RightWindow");
		windowCont.ProcessMode = ProcessModeEnum.WhenPaused;
	}

	public void ClearCenterWindow() 
	{
		var windowCont = GetNode<ReferenceRect>("CenterWindow");
		windowCont.GetChild(0).QueueFree();
		EnableRightWindowProcess();
	}
	
	/*Tutorials*/
	private void SetupTutorials()
	{
		var firstWindow = GetNode<PanelContainer>("TutorialWindows/FirstWindow");
		var secondWindow = GetNode<PanelContainer>("TutorialWindows/SecondWindow");
		var thirdWindow = GetNode<PanelContainer>("TutorialWindows/ThirdWindow");
		
		// Exit buttons
		var firstExit = firstWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		firstExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			game_state.CurrentShipyardTutorial = -1;
		};
		var secondExit = secondWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		secondExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			secondWindow.Visible = false;
			game_state.CurrentShipyardTutorial = -1;
		};
		var thirdExit = thirdWindow.GetNode<Button>("MarginContainer/VBox/ExitHBox/ExitButton");
		thirdExit.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			thirdWindow.Visible = false;
			game_state.CurrentShipyardTutorial = -1;
		};
		
		// Next buttons
		var firstNext = firstWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		firstNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			firstWindow.Visible = false;
			secondWindow.Visible = true;
			game_state.CurrentShipyardTutorial = 2;
		};
		var secondNext = secondWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		secondNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			secondWindow.Visible = false;
			thirdWindow.Visible = true;
			game_state.CurrentShipyardTutorial = 3;
		};
		var thirdNext = thirdWindow.GetNode<Button>("MarginContainer/VBox/MarginContainer/Button");
		thirdNext.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			thirdWindow.Visible = false;
			game_state.CurrentShipyardTutorial = -1;
		};

		switch (game_state.CurrentShipyardTutorial)
		{
			case 1:
				firstWindow.Visible = true;
				break;
			case 2:
				secondWindow.Visible = true;
				break;
			case 3:
				thirdWindow.Visible = true;
				break;
			default:
				firstWindow.Visible = false;
				secondWindow.Visible = false;
				thirdWindow.Visible = false;
				break;
		}
	}
	
}
