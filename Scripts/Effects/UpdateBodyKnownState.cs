using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.StarSystem;

namespace GloryOfRitiria.Scripts.Effects;

class UpdateBodyKnownState : EffectOnReturn
{
    private CelestialBodyState _state;
    
    public UpdateBodyKnownState(Mission mission, CelestialBodyState state, string desc = "Ship returns with scientific results") : base(mission, desc)
    {
        _state = state;
    }

    public override void ApplyEffect()
    {
        Mission.TargetBody.UpdateKnownState(_state);
    }
}