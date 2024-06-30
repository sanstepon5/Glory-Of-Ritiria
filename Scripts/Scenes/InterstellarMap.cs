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
}
