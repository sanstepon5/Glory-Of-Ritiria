using GloryOfRitiria.Scripts;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

namespace GloryOfRitiria.Scenes.Parts;

public partial class ShipScene : GridContainer
{
	private GlobalSignals _signals;
	public Ship Ship;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		_signals.Connect(nameof(_signals.ShipClicked), new Callable(this, nameof(_updateShipScene)));
		_signals.Connect(nameof(_signals.ShipStartedRoute), new Callable(this, nameof(_updateShipScene)));
	}

	private void _updateShipScene()
	{
		_setShipTextures();
	}
	
	private void _setShipTextures()
	{
		var shipButton = GetNode<TextureButton>("ShipContainer/MarginContainer/ShipButton");
		shipButton.TextureNormal = (Texture2D)GD.Load(Ship.GetImagePath());
		shipButton.TexturePressed = (Texture2D)GD.Load(Ship.GetImagePath());
		shipButton.TextureHover = (Texture2D)GD.Load(Ship.GetImagePath());
		shipButton.TextureHover = (Texture2D)GD.Load(Ship.GetImagePath());
	}
	
}
