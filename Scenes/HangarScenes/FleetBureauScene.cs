using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Utils;

public partial class FleetBureauScene : Node2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.EmitSignal(nameof(_signals.FleetBureauSceneOpened));
	}

}
