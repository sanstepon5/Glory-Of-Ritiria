using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;

public partial class QuestGoalMCont : MarginContainer
{
	[Export] private string QuestGoalText = "Achieve a Mission Goal";
	[Export] private QuestType questType;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalSignals signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		GetNode<RichTextLabel>("HBox/Label").Text = QuestGoalText;
		signals.Connect(nameof(signals.MaterialResChanged), new Callable(this, nameof(Get100ResGoal)));
	}
	
	private void Get100ResGoal(double amount)
	{
		if (questType != QuestType.Get100Res) return;
		if (amount >= 100)
		{
			GetNode<CheckBox>("HBox/CheckBox").ButtonPressed = true;
		}
	}

	private void ExplorePlanetGoal()
	{
		if (questType != QuestType.ExplorePlanet) return;
	}
	
	private void BuildShipGoal()
	{
		if (questType != QuestType.ExplorePlanet) return;
	}
}

public enum QuestType{
	Get100Res,
	ExplorePlanet,
	BuildShip
}
