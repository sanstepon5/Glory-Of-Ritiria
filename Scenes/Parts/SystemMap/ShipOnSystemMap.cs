using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.Parts.SystemMap;

public partial class ShipOnSystemMap : VBoxContainer
{
	private GlobalSignals _signals;
	public Ship Ship;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		_signals.Connect(nameof(_signals.ShipClicked), new Callable(this, nameof(_updateShipScene)));
		_signals.Connect(nameof(_signals.ShipStartedRoute), new Callable(this, nameof(_updateShipScene)));
	}

	public void Init(Ship ship)
	{
		Ship = ship;
		GetNode<Label>("ShipName").Text = ship.Name; // Better move that to the script too
		
		// Set textures of this ship instance (doing it here to avoid updating all instances)
		ship.SimpleUpdate();
		
		var textureButton = UpdateShipTextures();
		textureButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_handleShipButtonPress();
		};

	}

	private void _updateShipScene(Ship ship = null)
	{
		// if (ship is null || Ship == ship)
		UpdateShipTextures();
	}
	
	public TextureButton UpdateShipTextures()
	{
		var shipButton = GetNode<TextureButton>("ShipButton");
		shipButton.TextureNormal = (Texture2D)GD.Load(Ship.GetImagePath());
		shipButton.TexturePressed = (Texture2D)GD.Load(Ship.GetImagePath());
		shipButton.TextureHover = (Texture2D)GD.Load(Ship.GetImagePath());
		shipButton.TextureHover = (Texture2D)GD.Load(Ship.GetImagePath());
		return shipButton;
	}
	
	private void _handleShipButtonPress()
	{
		if (game_state.SelectedShip != null) GD.Print("Current selected ship: "+game_state.SelectedShip.Name);
		Ship.ChangeSelected();
		Ship.SimpleUpdate();

		_signals.EmitSignal(nameof(_signals.ShipClicked), Ship); // Update all ship scenes
		_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired)); // Update the topbar (really only need one part but later...)
	}
	
}
