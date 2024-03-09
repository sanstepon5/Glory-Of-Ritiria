using System.Collections.Generic;

namespace GloryOfRitiria.Scripts.ShipRelated;

/**Mission that a ship can carry out*/
public class Mission
{
    public string Name;
    public List<Effect> EffectsOnSuccess;

    public Mission(string name)
    {
        Name = name;
        EffectsOnSuccess = new List<Effect>();
    }

    public void AddEffect(Effect effect)
    {
        EffectsOnSuccess.Add(effect);
    }

    public void ExecuteEffects()
    {
        foreach (var effect in EffectsOnSuccess)
        {
            effect.ApplyEffect();
        }
    }
}