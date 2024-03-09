using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils.Events;

namespace GloryOfRitiria.Scripts.Scenes.Parts;

public class Choice
{
	public string Id;
	public string Desc; // Will be redundant when localisation files will be added, but for now it'll have to stay
	public List<Effect> Effects = new List<Effect>();

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
}
