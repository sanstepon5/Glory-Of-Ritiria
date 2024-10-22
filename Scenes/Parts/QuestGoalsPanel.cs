using Godot;
using System;

public partial class QuestGoalsPanel : Control
{
	private bool showGoalsNextClick = false;
	public override void _Ready()
	{
		GetNode<Button>("PCont/VBox/TitleMCont/TitleHBox/ShowHideButton")
			.Connect("pressed", new Callable(this, nameof(ShowHideQuestGoals)));
	}

	public void ShowHideQuestGoals()
	{
		GetNode<VBoxContainer>("PCont/VBox/GoalsVBox").Visible = showGoalsNextClick;
		GetNode<Label>("PCont/VBox/ThreePoints").Visible = !showGoalsNextClick;
		var iconName = "hide";
		if (showGoalsNextClick) iconName = "show";
		GetNode<Button>("PCont/VBox/TitleMCont/TitleHBox/ShowHideButton").Icon =
			(Texture2D)GD.Load($"res://Assets/GUI/Icons/32/{iconName}.png");
		
		showGoalsNextClick = !showGoalsNextClick;
	}
}
