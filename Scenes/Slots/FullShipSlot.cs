using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

namespace GloryOfRitiria.Scenes.Slots;

public partial class FullShipSlot : PanelContainer
{
	private GlobalSignals _signals;
	private Ship _ship;
	
	public override void _Ready()
	{
		AddChild(CreateTooltipController());
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
	}

	public void Init(Ship ship, GlobalSignals signals)
	{
		_ship = ship;
		_signals = signals;
		
		var slotImage = GetNode<TextureRect>("MCont/VBox/ShipImage");
		slotImage.Texture = (Texture2D)GD.Load(_ship.GetImagePath(ShipImageSize.Big));
		
		var slotName = GetNode<RichTextLabel>("MCont/VBox/NameLabel");
		// TODO: Add dynamic type icon
		slotName.Text = "[img]res://Assets/GUI/Icons/32/CameraReticle.png[/img]  " + _ship.Name;
		
		var slotLocation = GetNode<RichTextLabel>("MCont/VBox/LocationLabel");
		slotLocation.Text = "[img]"+ _ship.Location.GetSmallImage() + "[/img]  " + _ship.Location.Name;


		var button = GetNode<Button>("MCont/Button");
		button.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.FullSlotClicked), _ship);
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
		};
	}
	
	private TooltipController CreateTooltipController()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Utils/TooltipController.tscn");
		var inst = (TooltipController)scene.Instantiate();

		inst.Name = "TooltipController";
		inst.MinimumXSize = 300;
		inst.VisualsText = $"{_ship.Name} is currently docked on {_ship.Location.Name}. " +
						   $"[img]res://Assets/Icons/leftMouseClick32.png[/img] to see or outfit its modules";
		// TODO: This way mouse_entered works well but the tooltip is cut at the top
		inst.OwnerPath = new NodePath(GetNode<Button>("MCont/Button").GetPath());
		inst.EnterDelay = 0.3;
		inst.ExitDelay = 0.3;

		return inst;
	}
}
