using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.StarSystem;

public partial class HangarPlanetSelectionScene : MarginContainer
{
	private CelestialBody _body;
		
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
	}
	
	
	// Called by signal once Body is set-up
	public void Init(CelestialBody body, GlobalSignals signals)
	{
		_body = body;
		_signals = signals;
		
		var bodyName = GetNode<Label>("VBox/PlanetLabel");
		bodyName.Text = _body.Name;

		var shipyardBox = GetNode<VBoxContainer>("VBox/ShipyardsHBox/NamesVBox");
		foreach (var shipyard in _body.Shipyards)
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
				if (game_state.BodiesWithShipyards[i] == _body) index = i;
			}
			_signals.EmitSignal(nameof(_signals.ShipyardsBodyChanged), index);
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
		};
	}
}
