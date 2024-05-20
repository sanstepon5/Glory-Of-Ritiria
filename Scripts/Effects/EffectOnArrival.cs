using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.Effects;

abstract class EffectOnArrival : MissionEffect
{
    public EffectOnArrival(Mission mission, string desc) : base(mission, desc)
    {
    }
}