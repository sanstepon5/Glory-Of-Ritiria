using System.Collections.Generic;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

/**Mission that a ship can carry out*/
public abstract class Mission
{
    public string Name;
    public List<Effect> EffectsOnSuccess;

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