using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

public partial class SlotList : GridContainer
{
	public CelestialBody CurrentBody;
	private ShipyardList _slots; // Should be ordered
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_slots = new ShipyardList();
		//InitDefaultShipyards();
		
		_signals.Connect(nameof(_signals.ShipBuildStarted), new Callable(this, nameof(UpdateSlots)));
		_signals.Connect(nameof(_signals.TurnPassed), new Callable(this, nameof(UpdateSlots)));
		_signals.Connect(nameof(_signals.ShipyardsSceneOpened), new Callable(this, nameof(InitDefaultShipyards)));
	}

	// Called by signal to show shipyards of a different body
	public void ChangeCurrentBody(CelestialBody body)
	{
		CurrentBody = body;
		UpdateSlots();
	}
	
	// Init with the first body in list of bodies with shipyards
	public void InitDefaultShipyards()
	{
		CurrentBody = game_state.BodiesWithShipyards[0];
		Init();
	}
	
	// Called either by signal or in this class
	public void Init()
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
			AddChild(BuildFullSlot(ship));
		}
	}
	

	// For docked ships that can be modified on the planet
	public PanelContainer BuildFullSlot(Ship ship)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/FullShipSlot.tscn");
		var inst = (PanelContainer)scene.Instantiate();

		
		var slotName = inst.GetNode<RichTextLabel>("MCont/VBox/TopHBox/NameLabel");
		slotName.Text = ship.Name;
		
		var slotLocation = inst.GetNode<RichTextLabel>("MCont/VBox/BottomHBox/LocationLabel");
		slotLocation.Text = ship.Location.Name;

		
		return inst;
	}

	public PanelContainer BuildBuildingSlot(Shipyard slot)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/ProgressShipSlot.tscn");
		var inst = (PanelContainer)scene.Instantiate();

		
		var shipName = inst.GetNode<RichTextLabel>("MCont/VBox/TopHBox/NameLabel");
		shipName.Text = slot.Ship.Name;
		
		var shipyardName = inst.GetNode<RichTextLabel>("MCont/VBox/ShipyardName");
		shipyardName.Text = "[img]"+ slot.Location.GetSmallImage() + "[/img]  " + slot.ShipyardName;
		
		var progressBar = inst.GetNode<ProgressBar>("MCont/VBox/CenterVBox/MCont/ProgressBar");
		var progressValue =  (double) slot.CurrentProgress / slot.TurnCost * 100;
		progressBar.Value = progressValue;
		
		return inst;
	}

	public PanelContainer BuildEmptySlot(Shipyard slot)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/EmptyShipSlot.tscn");
		var inst = (PanelContainer)scene.Instantiate();
		
		var shipyardName = inst.GetNode<RichTextLabel>("MCont/VBox/ShipyardName");
		shipyardName.Text = "[img]"+ slot.Location.GetSmallImage() + "[/img]  " + slot.ShipyardName;
		
		var addButton = inst.GetNode<TextureButton>("MCont/VBox/AddButton");
		addButton.Pressed += () => RequestOpenConstructionWindow(slot);

		
		return inst;
	}
	
	public void UpdateSlots()
	{
		foreach (var child in GetChildren())
		{
			child.QueueFree();
		}

		_slots = new();
		Init();
	}
	
	public void RequestOpenConstructionWindow(Shipyard shipyard)
	{
		_signals.EmitSignal(nameof(_signals.ConstructionWindowRequested), shipyard);
	}
}
