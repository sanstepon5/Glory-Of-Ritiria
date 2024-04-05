using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class ShipOutfittingWindow : PanelContainer
{
	private Ship _ship;

	private GlobalSignals _signals;
	public override void _Ready()
	{
		GetTree().Paused =  true;
	}
	
	
	public void Init(Ship ship, GlobalSignals signals)
	{
		_signals = signals;
		_ship = ship;
		
		var shipNameLabel = GetNode<RichTextLabel>("MCont/VBox/NameHBox/MCont/ShipName");
		shipNameLabel.Text = _ship.Name;
		
		var exitButton = GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			GetTree().Paused = false;
			QueueFree();
		};

		var cargoView = GetNode<CargoView>("MCont/VBox/CargoVBox/ImageMargin/CargoView");
		cargoView.Signals = _signals;
		cargoView.Init(_ship);
		
		var outfitButton = GetNode<Button>("MCont/VBox/ButtonMargin/OutfitButton");
		outfitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			GetTree().Paused = false;
			QueueFree();
			
			// Ship.blablabla = blablabla			
		};
	}


}
