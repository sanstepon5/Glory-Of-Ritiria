using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils.Events;

namespace GloryOfRitiria.Scripts.Scenes.Parts;

public class ChoiceButton
{
	public string desc;
	public List<Effect> Effects = new List<Effect>();

	public ChoiceButton(string desc)
	{
		this.desc = desc;
	}
	
	public void ApplyEffects()
	{
		foreach (var effect in Effects)
		{
			effect.ApplyEffect();
		}
	}
}
