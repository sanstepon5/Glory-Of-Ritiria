using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class ShipOutfittingWindow : PanelContainer
{
	private Ship _ship;

	private GlobalSignals _signals;
	private RichTextLabel _shipNameLabel;
	private RichTextLabel _capacityLabel;
	private CargoView _cargoView;

	public override void _Ready()
	{
		GetTree().Paused =  true;
		_signals.Connect(nameof(_signals.OutfitWindowUpdateRequired), new Callable(this, nameof(Update)));
		_signals.Connect(nameof(_signals.CargoButtonClicked), new Callable(this, nameof(Update)));
	}
	
	
	public void Init(Ship ship, GlobalSignals signals)
	{
		_signals = signals;
		_ship = ship;
		
		_shipNameLabel = GetNode<RichTextLabel>("MCont/VBox/NameHBox/MCont/ShipName");
		_shipNameLabel.Text = _ship.Name;

		_capacityLabel = GetNode<RichTextLabel>("MCont/VBox/CargoVBox/Label");
		_capacityLabel.Text = "Current cargo (" + ship.GetCargo().Count + "/" + ship.GetCargoCapacity() + "):";
		
		var exitButton = GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_cargoView.ResetGameState();
			GetTree().Paused = false;
			QueueFree();
		};

		_cargoView = GetNode<CargoView>("MCont/VBox/CargoVBox/ImageMargin/CargoView");
		_cargoView.Signals = _signals;
		_cargoView.Init(_ship);
		
		var outfitButton = GetNode<Button>("MCont/VBox/ButtonMargin/OutfitButton");
		outfitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_cargoView.OutfitCargo(_ship);
			GetTree().Paused = false;
			QueueFree();
		};
	}


	public void Update()
	{
		
		_capacityLabel.Text = "Current cargo (" + _cargoView.GetCargoCount() + "/" + _ship.GetCargoCapacity() + "):";
		
		_cargoView.Update();
	}
}
