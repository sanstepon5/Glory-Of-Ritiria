using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class CargoSelectWindow : PanelContainer
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		if (_signals == null)
		{
			_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		}
		_signals.Connect(nameof(_signals.CargoSelectedForOutfit), new Callable(this, nameof(_removeWindow)));
	}

	public void Init(GlobalSignals signals)
	{
		_signals = signals;
		
		var cargoVBox = GetNode<VBoxContainer>("VBox/MarginCont/ScrollCont/VBox");
		foreach (var cargo in game_state.CargoStorage)
		{
			cargoVBox.AddChild(_initCargoOption(cargo));
		}

		
		var exitButton = GetNode<Button>("VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.ThirdLevelProcessExited));
			QueueFree();
		};

		_signals.EmitSignal(nameof(_signals.ThirdLevelProcessEntered));
	}

	private CargoOption _initCargoOption(Cargo cargo)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/CargoOption.tscn");
		var inst = (CargoOption)scene.Instantiate();
		inst.Init(cargo, _signals);
		
		return inst;
	}

	private void _removeWindow(string foo) // I think it's still better than sending two signals
	{
		_signals.EmitSignal(nameof(_signals.ThirdLevelProcessExited));
		QueueFree();
	}
}
