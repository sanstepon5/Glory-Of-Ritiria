using Godot;
using System;
using GloryOfRitiria;

// TODO: Right now different hangar scenes share nothing in common which is ok... Except for this menu.
// It should be persistent between hangar scenes, only changing buttons because reloading it looks annoying
public partial class HangarMenu : PanelContainer
{
	private GlobalSignals _signals;

	[Export] public CurrentHangarScene Scene;
	private Button _backButton;
	private Button _shipyardsButton;
	private Button _fleetButton;
	private Button _designButton;

	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");

		_backButton = GetNode<Button>("VBox/BackMargin/Button");
		_backButton.Pressed += () => _signals.EmitSignal(nameof(_signals.PallyriaClicked));
		
		_shipyardsButton = GetNode<Button>("VBox/ConstructionMargin/Button");
		_shipyardsButton.Pressed += () => _signals.EmitSignal(nameof(_signals.ShipyardsButtonClicked));
		
		_fleetButton = GetNode<Button>("VBox/FleetMargin/Button");
		_fleetButton.Pressed += () => _signals.EmitSignal(nameof(_signals.FleetBureauButtonClicked));
		
		_designButton = GetNode<Button>("VBox/DesignMargin/Button");
		//_designButton.Pressed += () => _signals.EmitSignal(nameof(_signals.FleetBureauButtonClicked));

		Init();
	}

	// Depends on the scene set up in the editor
	public void Init()
	{
		switch (Scene)
		{
			case CurrentHangarScene.Shipyards:
				_shipyardsButton.Disabled = true; break;
			case CurrentHangarScene.Fleet:
				_fleetButton.Disabled = true; break;
			case CurrentHangarScene.Design:
				_designButton.Disabled = true; break;
		}
	}

}

public enum CurrentHangarScene
{
	Shipyards,
	Fleet,
	Design
}
