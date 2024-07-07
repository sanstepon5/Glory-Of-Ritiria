using System;
using GloryOfRitiria.Scenes.Slots;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.StarSystem;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;
using Shipyard = GloryOfRitiria.Scripts.ShipRelated.Shipyard;

namespace GloryOfRitiria.Scenes.HangarScenes;

public partial class SlotList : GridContainer
{
	public CelestialBody CurrentBody;
	public CurrentHangarScene CurrentScene;
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		_signals.Connect(nameof(_signals.ShipBuildStarted), new Callable(this, nameof(UpdateSlots)));
		_signals.Connect(nameof(_signals.TurnPassed), new Callable(this, nameof(UpdateSlots)));
		_signals.Connect(nameof(_signals.ShipyardsSceneOpened), new Callable(this, nameof(InitDefaultShipyards)));
		_signals.Connect(nameof(_signals.FleetBureauSceneOpened), new Callable(this, nameof(InitFleetBureau)));
		_signals.Connect(nameof(_signals.ShipyardsBodyChanged), new Callable(this, nameof(ChangeCurrentBody)));
	}

	// Called by signal to show shipyards of a different body
	public void ChangeCurrentBody(int index)
	{
		CurrentBody = game_state.BodiesWithShipyards[index];
		UpdateSlots();
	}
	
	// Init with the first body in list of bodies with shipyards
	public void InitDefaultShipyards()
	{
		CurrentScene = CurrentHangarScene.Shipyards;
		CurrentBody = game_state.BodiesWithShipyards[0];
		InitShipyards();
	}
	
	// Called either by signal or in this class
	private void InitShipyards()
	{
		foreach (var shipyard in CurrentBody.Shipyards)
		{
			switch (shipyard.State)
			{
				case SlotState.Busy:
					AddChild(BuildBuildingSlot(shipyard));
					break;
				case SlotState.Empty:
					AddChild(BuildEmptySlot(shipyard));
					break;
			}
		}

		foreach (var ship in CurrentBody.ShipsInOrbit)
		{
			if (!ship.IsInRoute())
				AddChild(BuildFullSlot(ship));
		}
	}
	
	// Called by signal
	public void InitFleetBureau()
	{
		CurrentScene = CurrentHangarScene.Fleet;
		
		foreach (var ship in game_state.AllShips)
		{	// Shows ships that started moving too to be able to cancel the order
			AddChild(BuildFullSlot(ship));
		}
	}
	

	// For docked ships that can be modified on the planet
	public FullShipSlot BuildFullSlot(Ship ship)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/FullShipSlot.tscn");
		var inst = (FullShipSlot)scene.Instantiate();
		inst.Init(ship, _signals);
		
		return inst;
	}

	public PanelContainer BuildBuildingSlot(Shipyard slot)
	{
		slot.UpdateBuildingSpeed();
		
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/ProgressShipSlot.tscn");
		var inst = (ProgressShipSlot)scene.Instantiate();
		inst.Init(slot);
		return inst;
	}

	public PanelContainer BuildEmptySlot(Shipyard slot)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/EmptyShipSlot.tscn");
		var inst = (EmptyShipSlot)scene.Instantiate();
		
		inst.Init(slot);
		
		var addButton = inst.GetNode<TextureButton>("MCont/VBox/AddButton");
		addButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			RequestOpenConstructionWindow(slot);
		};
		
		return inst;
	}
	
	public void UpdateSlots()
	{
		foreach (var child in GetChildren())
		{
			child.QueueFree();
		}

		if (CurrentScene == CurrentHangarScene.Shipyards)
			InitShipyards();
		else
			InitFleetBureau();
		
	}
	
	public void RequestOpenConstructionWindow(Shipyard shipyard)
	{
		_signals.EmitSignal(nameof(_signals.ConstructionWindowRequested), shipyard);
	}
}