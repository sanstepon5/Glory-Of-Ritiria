using GloryOfRitiria.Scenes.HangarScenes.Windows;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using GloryOfRitiria.Scripts.ShipRelated;

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
	}

	public void BuildConstructionWindow(Shipyard shipyard)
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/ShipConstructionWindow.tscn");
		var inst = (ShipConstructionWindow)scene.Instantiate();
		inst.Init(shipyard, _signals);
		
		windowCont.AddChild(inst);
	}	
	
	public void BuildOutfittingWindow(Ship ship)
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/ShipOutfittingWindow.tscn");
		var inst = (ShipOutfittingWindow)scene.Instantiate();
		inst.Init(ship, _signals);
		
		windowCont.AddChild(inst);
	}
	
	public void BuildCargoSelectWindow()
	{
		var windowCont = GetNode<ReferenceRect>("CenterWindow");
		
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
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		windowCont.ProcessMode = ProcessModeEnum.Disabled;
	}
	public void EnableRightWindowProcess()
	{
		var windowCont = GetNode<ReferenceRect>("RightWindow");
		windowCont.ProcessMode = ProcessModeEnum.WhenPaused;
	}

	public void ClearCenterWindow() 
	{
		var windowCont = GetNode<ReferenceRect>("CenterWindow");
		windowCont.GetChild(0).QueueFree();
		EnableRightWindowProcess();
	}
	
}
