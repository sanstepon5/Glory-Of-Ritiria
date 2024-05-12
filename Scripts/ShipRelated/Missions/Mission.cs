using System.Collections.Generic;
using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

/**Mission that a ship can carry out*/
public abstract class Mission
{
    public string Name;
    public List<MissionEffect> EffectsOnSuccess;
    
    private Cargo _associatedCargo;
    
    public void SetAssociatedCargo(Cargo cargo)
    {
        _associatedCargo = cargo;
    }
    
    

    public void AddEffect(MissionEffect effect)
    {
        EffectsOnSuccess.Add(effect);
    }

    public void ExecuteEffects()
    {
        _associatedCargo.DecreaseDurability();
        foreach (var effect in EffectsOnSuccess)
        {
            effect.ApplyEffect();
        }
    }
}