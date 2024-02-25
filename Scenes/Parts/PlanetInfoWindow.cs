using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;

public partial class PlanetInfoWindow : PanelContainer
{
	private GlobalSignals _signals;
	public CelestialBody Body;
	public override void _Ready()
	{
		// _signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		// _signals.Connect(nameof(_signals.ShipStartedRoute), new Callable(this, nameof(UpdateWindow)));
	}

	public void UpdateWindow()
	{
		var sendButton = GetNode<Button>("MCont/VBox/SendShipMargin/SendShipButton");
		 if (game_state.SelectedShip.IsInRouteTo(Body))
		 {
		 	sendButton.Disabled = true;
		 	sendButton.Text = game_state.SelectedShip.Name + " in route here";
		 }
		else sendButton.Disabled = false;
	}
	
}
