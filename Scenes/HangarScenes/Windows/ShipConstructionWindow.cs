using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scenes.HangarScenes.Windows;

public partial class ShipConstructionWindow : PanelContainer
{
	private Shipyard _shipyard;

	private GlobalSignals _signals;

	private double _shipBuildSpeedModifier;
	private int _buildCost;
	private double _costModifier;
	private int _turnCost;
	
	private OptionButton _sizeButton;
	private RichTextLabel _timeCostLabel;
	private RichTextLabel _buildCostLabel;
	private GridContainer _priorityGrid;
	private Button _buildButton;

	public override void _Ready()
	{
		GetTree().Paused =  true;
	}
		
	public void Init(Shipyard shipyard, GlobalSignals signals)
	{
		_signals = signals;
		_shipyard = shipyard;
		_shipBuildSpeedModifier = 1.0;
		
		_priorityGrid = GetNode<GridContainer>("MCont/VBox/PriorityHBox/MCont/GridCont");
		_timeCostLabel = GetNode<RichTextLabel>("MCont/VBox/TimeCostHBox/MarginContainer/CostLabel");
		_buildCostLabel = GetNode<RichTextLabel>("MCont/VBox/CostHBox/MarginContainer/CostLabel");
		_sizeButton = GetNode<OptionButton>("MCont/VBox/SizeHBox/MCont/OptionButton");
		
		_buildButton = GetNode<Button>("MCont/VBox/ButtonMargin/BuildButton");
		
		
		var constructionLocation = GetNode<RichTextLabel>("MCont/VBox/LocationHBox/MarginContainer/LocationLabel");
		constructionLocation.Text = "[img]res://Assets/GUI/Icons/32/liveablePlanet.png[/img]  " + _shipyard.Location.Name;

		PriorityCheckBoxInit();


		UpdateBuildCost(_sizeButton, _sizeButton.Selected); // init cost based on default choice
		_updateResCostLabel();
		_sizeButton.ItemSelected += index => UpdateBuildCost(_sizeButton, (int)index);
		
		var exitButton = GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			GetTree().Paused = false;
			QueueFree();
		};
		
		
		_buildButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			
			var shipName = GetNode<LineEdit>("MCont/VBox/NameHBox/MCont/TextEdit");
			
			var ship = new Ship(shipName.Text, _shipyard.Location, true);
			SetShipSize(_sizeButton, ship);
			
			_shipyard.StartConstruction(ship, _turnCost);
			_signals.EmitSignal(nameof(_signals.ShipBuildStarted));
			
			game_state.Res1 -= _buildCost;
			_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
			
			GetTree().Paused = false;
			QueueFree();
		};


		_updateTimeCostLabel();
	}

	private void SetShipSize(OptionButton sizeButton, Ship ship)
	{
		switch (sizeButton.GetItemText(sizeButton.Selected).Trim())
		{
			case "Small":
				ship.Design.Size = ShipFrameSize.Small;
				break;
			case "Medium":
				ship.Design.Size = ShipFrameSize.Medium;
				break;
			case "Large":
				ship.Design.Size = ShipFrameSize.Large;
				break;
			default:
				GD.PrintErr("Incorrect ship size selected : " + sizeButton.GetItemText(sizeButton.Selected).Trim());
				break;
		}
	}

	private void PriorityCheckBoxInit()
	{
		var checkBox1 = _priorityGrid.GetNode<CheckBox>("CheckBoxHigh");
		checkBox1.Pressed += () =>
		{
			SetShipBuildSpeedModifier(1.2);
		};
		var checkBox2 = _priorityGrid.GetNode<CheckBox>("CheckBoxMedium");
		checkBox2.Pressed += () =>
		{
			SetShipBuildSpeedModifier(1.0);
		};
		var checkBox3 = _priorityGrid.GetNode<CheckBox>("CheckBoxLow");
		checkBox3.Pressed += () =>
		{
			SetShipBuildSpeedModifier(0.8);
		};
	}

	private void UpdateBuildCost(OptionButton sizeButton, int index)
	{
		const int shipCost = 100; // should come from the ship's design
		switch (sizeButton.GetItemText(index).Trim())
		{
			case "Small":
				_costModifier = -0.5;
				SetBuildCost((int)Math.Ceiling(shipCost + (shipCost * _costModifier)));
				break;
			case "Medium":
				_costModifier = 0.0;
				SetBuildCost((int)Math.Ceiling(shipCost + (shipCost * _costModifier)));
				break;
			case "Large":
				_costModifier = 0.5;
				SetBuildCost((int)Math.Ceiling(shipCost + (shipCost * _costModifier)));
				break;
			default:
				GD.PrintErr("Unknown ship size selected : " + sizeButton.GetItemText(sizeButton.Selected).Trim());
				break;
		}
	}

	private void SetTurnCost(int cost)
	{
		_turnCost = cost;
		_updateTimeCostLabel();
	}

	private void SetShipBuildSpeedModifier(double modifier)
	{
		_shipBuildSpeedModifier = modifier;
		_updateTurnCost();
	}

	private void SetBuildCost(int cost)
	{
		_buildCost = cost;
		_updateTurnCost();
		_updateResCostLabel();
		_buildButton.Disabled = game_state.Res1 < _buildCost;
	}
	

	private void _updateTurnCost()
	{
		SetTurnCost((int) Math.Ceiling((_buildCost * _shipyard.BuildingSpeed)* (1/_shipBuildSpeedModifier)));
	}
	
	private void _updateResCostLabel()
	{
		var text = "";
		if (game_state.Res1 < _buildCost)
		{
			text += "[color=red]";
		}

		text += _buildCost + "  [img]res://Assets/GUI/Icons/32/Bricks.png[/img]";
		
		if (game_state.Res1 < _buildCost)
		{
			text += "[/color]";
		}

		_buildCostLabel.Text = text;
	}

	private void _updateTimeCostLabel()
	{
		var shipyardEfficiency = _shipyard.BuildingEfficiency + (_shipyard.BuildingEfficiency * (game_state.ScientificRes / 100));
		var timeCostWithModifiers = Math.Ceiling(_turnCost / shipyardEfficiency);
		_timeCostLabel.Text = timeCostWithModifiers + "  [img]res://Assets/GUI/Icons/32/time.png[/img]";
	}
}
