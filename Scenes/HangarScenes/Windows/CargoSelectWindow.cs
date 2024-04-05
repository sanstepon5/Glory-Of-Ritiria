using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class CargoSelectWindow : PanelContainer
{
	private GlobalSignals _signals;
	public List<Cargo> InitialCargoList;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");

		var cargoVBox = GetNode<VBoxContainer>("VBox/MarginCont/ScrollCont/VBox");

		foreach (var cargo in game_state.CargoStorage)
		{
			cargoVBox.AddChild(_initCargoOption(cargo));
		}
	}

	private PanelContainer _initCargoOption(Cargo cargo)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/HangarScenes/Windows/CargoOption.tscn");
		var inst = (CargoOption)scene.Instantiate();
		inst.Init(cargo, _signals);
		
		return inst;
	}
}
