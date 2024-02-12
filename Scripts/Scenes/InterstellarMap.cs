using Godot;
using System;
using System.Collections.Generic;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

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
		
	}

	public void BuildSystem(StarSystemInfo systemInfo)
	{
		var starSystem = GD.Load<PackedScene>("res://Scenes/Utils/StarSystem.tscn");
		var starSystemInst = (VBoxContainer) starSystem.Instantiate();
		
		starSystemInst.GetNode<RichTextLabel>("SystemName").Text = systemInfo.Name;
		
		// Position the system relative to the center of the map (which is Detnura)
		var centerVBox = GetNode<VBoxContainer>("DetnuraVBox");
		starSystemInst.Position = systemInfo.GetPositionOnPlan(centerVBox.Position); // + centerVBox.Size/2
		

		var systemButton = starSystemInst.GetNode<TextureButton>("MarginContainer/StarButton");

		systemButton.Pressed += () =>
		{
			GD.Print("StarViewRequested Emitted");
			_signals.EmitSignal(nameof(_signals.StarViewRequested), systemInfo);
		};
		
		AddChild(starSystemInst);
	}

	// It's better to somehow merge it into BuildSystem since the only difference with any other system
	// Is it's position in the center and of an anchor to orbit lines but not today. 
	// public void BuildDetnura()
	// {
	// 	var systemButton = GetNode<TextureButton>("DetnuraVBox/MarginContainer/DetnuraButton");
	// 	systemButton.Pressed += () => _signals.EmitSignal(nameof(_signals.DetnuraSystemRequested));
	// }
	
	
}
