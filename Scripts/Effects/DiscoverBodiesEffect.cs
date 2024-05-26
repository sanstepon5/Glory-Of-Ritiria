using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.Effects;

internal class DiscoverBodiesEffect : EffectOnArrival
{
    public DiscoverBodiesEffect(Mission mission, string desc = "idunnolol") : base(mission, desc)
    {
    }
    
    public override void ApplyEffect()
    {
        if (Mission.TargetBody is not Star targetStar) return;
        foreach (var body in targetStar.Bodies)
        {
            body.DiscoverBody();
        }
    }
}