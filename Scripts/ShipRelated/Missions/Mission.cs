using System.Collections.Generic;
using GloryOfRitiria.Scripts.Effects;
using GloryOfRitiria.Scripts.StarSystem;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

/**Mission that a ship can carry out*/
public abstract class Mission
{
    public string Name;

    private List<MissionEffect> _effectsOnArrival;
    private List<MissionEffect> _effectsOnReturn;

    public CelestialBody TargetBody;

    
    private Cargo _associatedCargo;

    protected Mission(string name)
    {
        Name = name;
        _effectsOnArrival = new List<MissionEffect>();
        _effectsOnReturn = new List<MissionEffect>();
    }

    public void SetAssociatedCargo(Cargo cargo)
    {
        _associatedCargo = cargo;
    }

    public void SetTargetBody(CelestialBody body)
    {
        // foreach (var effect in _effectsOnArrival)
        // {
        //     effect.TargetBody = body;
        // }
        TargetBody = body;
    }

    public void AddEffectOnArrival(MissionEffect effect)
    {
        _effectsOnArrival.Add(effect);
    }
    
    public void AddEffectOnReturn(MissionEffect effect)
    {
        _effectsOnReturn.Add(effect);
    }

    public void ExecuteEffectsOnArrival()
    {
        _associatedCargo.DecreaseDurability();
        foreach (var effect in _effectsOnArrival)
        {
            effect.ApplyEffect();
        }
    }
    public void ExecuteEffectsOnReturn()
    {
        // _associatedCargo.DecreaseDurability();
        foreach (var effect in _effectsOnReturn)
        {
            effect.ApplyEffect();
        }
    }
}