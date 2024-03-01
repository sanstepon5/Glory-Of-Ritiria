using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;

public partial class HangarPlanetSelectionScene : MarginContainer
{

	// Should be set-up when instantiating this in code
	public CelestialBody Body;
		
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_signals.Connect(nameof(_signals.ShipyardsSceneOpened), new Callable(this, nameof(Init)));
	}
	
	
	// Called by signal once Body is set-up
	public void Init()
	{
		var bodyName = GetNode<Label>("VBox/PlanetLabel");
		bodyName.Text = Body.Name;

		var shipyardBox = GetNode<VBoxContainer>("VBox/ShipyardsHBox/NamesVBox");
		foreach (var shipyard in Body.Shipyards)
		{
			var shipyardLabel = new Label();
			shipyardLabel.Text = shipyard.ShipyardName;
			shipyardBox.AddChild(shipyardLabel);
		}

		var button = GetNode<Button>("VBox/ShipyardsHBox/ButtonMargin/Button");
		button.Pressed	+= () =>
		{
			var index = 0;
			// There will be only a handful of planets with shipyards so it shouldn't be a problem
			for (int i = 0; i < game_state.BodiesWithShipyards.Count; i++)
			{
				if (game_state.BodiesWithShipyards[i] == Body) index = i;
			}
			_signals.EmitSignal(nameof(_signals.ShipyardsBodyChanged), index);
		};
	}
}
