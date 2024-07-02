using System.Collections.Generic;
using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.Scenes.Parts;

public class Choice
{
	public string Id;
	public string Desc; // Will be redundant when localisation files will be added, but for now it'll have to stay
	public string TooltipText; // Text that will be shown before the effects in the tooltip
	public List<EventEffect> Effects = new();

	public Choice(string id, string desc)
	{
		Id = id;
		Desc = desc; 
	}

	// Mainly for parsing from file
	public Choice()
	{
		
	}
	
	public void ApplyEffects()
	{
		foreach (var effect in Effects)
		{
			effect.ApplyEffect();
		}
	}

	public string GetFullTooltipText()
	{
		var res = "[i]When chosen will have following effects:[/i]\n";

		foreach (var effect in Effects)
		{
			var text = effect.GetTooltipText();
			if (text != "")
			{
				res += " * " + text + "\n";
			}
			
		}
		

		return res;
	}
}
