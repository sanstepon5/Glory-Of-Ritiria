using System;
using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scenes.Parts;

public partial class TopBar : Panel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var yearLabel = GetNode<Label>("CurrentYear");
		yearLabel.Text = GetCurrentDateString();
		
		// Res 1 update
		var res1 = GetNode<HBoxContainer>("ResourceContainer/Res1");
		var res1Text = res1.GetNode<RichTextLabel>("ResText");
		res1Text.Text = "0\n[color=red]+ 0[/color]";
		
		var scientific = GetNode<HBoxContainer>("ResourceContainer/ScienceRes");
		var scientificText = scientific.GetNode<RichTextLabel>("ResText");
		scientificText.Text = "[color=red]0%[/color]";
		
		var political = GetNode<HBoxContainer>("ResourceContainer/PoliticalRes");
		var politicalText = political.GetNode<RichTextLabel>("ResText");
		politicalText.Text = "[color=red]0%[/color]";
		
		var shipName = GetNode<RichTextLabel>("SelectedShip/VBox/MarginCont/HBox/ShipName");
		shipName.Text = "No ship selected";
		var planetName = GetNode<RichTextLabel>("SelectedShip/VBox/MarginCont2/HBox/PlanetName");
		planetName.Text = "...";
		
	}
	
	public void Update()
	{
		var yearLabel = GetNode<Label>("CurrentYear");
		yearLabel.Text = GetCurrentDateString();
		
		// Res 1 update
		var res1 = GetNode<HBoxContainer>("ResourceContainer/Res1");
		var res1Text = res1.GetNode<RichTextLabel>("ResText");
		if (game_state.Res1Rate >= 0)
			res1Text.Text = "" + game_state.Res1 + "[color=green] + "+ game_state.Res1Rate+"[/color]";
		else
			res1Text.Text = "" + game_state.Res1 + "\n[color=red] - "+ Math.Abs(game_state.Res1Rate)+"[/color]";
		
		// Science update
		var scientific = GetNode<HBoxContainer>("ResourceContainer/ScienceRes");
		var scientificText = scientific.GetNode<RichTextLabel>("ResText");
		if 
			(game_state.ScientificRes > 5) scientificText.Text = "" + game_state.ScientificRes + "%";
		else 
			scientificText.Text = "[color=red]" + game_state.ScientificRes + "%[/color]";
		
		// Political power update
		var political = GetNode<HBoxContainer>("ResourceContainer/PoliticalRes");
		var politicalText = political.GetNode<RichTextLabel>("ResText");
		if (game_state.PoliticalRes > 5) 
			politicalText.Text = "" + game_state.PoliticalRes + "%";
		else 
			politicalText.Text = "[color=red]" + game_state.PoliticalRes + "%[/color]";
		
		// Selected Ship update
		var shipName = GetNode<RichTextLabel>("SelectedShip/VBox/MarginCont/HBox/ShipName");
		shipName.Text = "No ship selected";
		var planetName = GetNode<RichTextLabel>("SelectedShip/VBox/MarginCont2/HBox/PlanetName");
		planetName.Text = "...";
		if (game_state.SelectedShip != null)
		{
			shipName.Text = game_state.SelectedShip.Name;
			planetName.Text = game_state.SelectedShip.Location.Name;
		}
	}
	
	private static string GetCurrentDateString()
	{
		var nbWeeks = game_state.CurrentTurn;
		var pallyriaYear = 970 + nbWeeks / (4 * 12);
		var earthYear = 2017 + nbWeeks / (4 * 12);
		var month = 1 + (nbWeeks/4) % 12;
		var week = 1 + nbWeeks % 4;

		var res = $"Week {week}, Month {month}, {pallyriaYear} APE\n({earthYear} CE)";
		return res;
	}
}
