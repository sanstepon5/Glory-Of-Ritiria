using Godot;
using System;
using GloryOfRitiria;

public partial class HangarMenu : PanelContainer
{
	private GlobalSignals _signals;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");

		var backButton = GetNode<Button>("VBox/BackMargin/Button");
		backButton.Pressed += () => _signals.EmitSignal(nameof(_signals.PallyriaClicked));
	}

}
