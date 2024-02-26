using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

public partial class SlotList : GridContainer
{
	private SlotCollection _slots; // Should be ordered
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		_slots = game_state.ShipConstructionSlots;
		UpdateSlots();
		
		_signals.Connect(nameof(_signals.ShipBuildStarted), new Callable(this, nameof(UpdateSlots)));
		_signals.Connect(nameof(_signals.TurnPassed), new Callable(this, nameof(UpdateSlots)));
	}
	
	public void FillSlots()
	{
		foreach (var constructionSlot in _slots)
		{
			switch (constructionSlot.State)
			{
				case SlotState.Full:
					AddChild(BuildFullSlot(constructionSlot));
					break;
				case SlotState.Building:
					AddChild(BuildBuildingSlot(constructionSlot));
					break;
				case SlotState.Empty:
					AddChild(BuildEmptySlot(constructionSlot));
					break;
				case SlotState.Locked:
					AddChild(BuildLockedSlot());
					break;
			}
		}
	}

	public PanelContainer BuildFullSlot(ShipConstructionSlot slot)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/FullShipSlot.tscn");
		var inst = (PanelContainer)scene.Instantiate();

		
		var slotName = inst.GetNode<RichTextLabel>("MCont/VBox/TopHBox/NameLabel");
		slotName.Text = slot.Ship.Name;
		
		var slotLocation = inst.GetNode<RichTextLabel>("MCont/VBox/BottomHBox/LocationLabel");
		slotLocation.Text = slot.Ship.Location.Name;

		
		return inst;
	}

	public PanelContainer BuildBuildingSlot(ShipConstructionSlot slot)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/ProgressShipSlot.tscn");
		var inst = (PanelContainer)scene.Instantiate();

		
		var slotName = inst.GetNode<RichTextLabel>("MCont/VBox/TopHBox/NameLabel");
		slotName.Text = slot.Ship.Name;
		
		var slotLocation = inst.GetNode<RichTextLabel>("MCont/VBox/BottomHBox/LocationLabel");
		
		slotLocation.Text = slot.Ship.Location.Name;
		
		var progressBar = inst.GetNode<ProgressBar>("MCont/VBox/CenterVBox/MCont/ProgressBar");
		var progressValue =  (double) slot.CurrentProgress / slot.TurnCost * 100;
		progressBar.Value = progressValue;
		
		return inst;
	}

	public PanelContainer BuildEmptySlot(ShipConstructionSlot slot)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/EmptyShipSlot.tscn");
		var inst = (PanelContainer)scene.Instantiate();

		
		var slotLocation = inst.GetNode<RichTextLabel>("MCont/VBox/BottomHBox/LocationLabel");
		slotLocation.Text = slot.Location.Name;
		
		var addButton = inst.GetNode<TextureButton>("MCont/VBox/AddButton");
		addButton.Pressed += () => RequestOpenConstructionWindow(slot);

		
		return inst;
	}

	public PanelContainer BuildLockedSlot()
	{
		// TODO
		var scene = GD.Load<PackedScene>("res://Scenes/Slots/LockedShipSlot.tscn");
		var inst = (PanelContainer)scene.Instantiate();
		
		return inst;
	}

	public void UpdateSlots()
	{
		foreach (var child in GetChildren())
		{
			child.QueueFree();
		}

		_slots = game_state.ShipConstructionSlots;
		FillSlots();
	}
	
	public void RequestOpenConstructionWindow(ShipConstructionSlot slot)
	{
		_signals.EmitSignal(nameof(_signals.ConstructionWindowRequested), slot);
	}
}
