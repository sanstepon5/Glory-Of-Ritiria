using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.Effects;

class ExploreBodyEffect : EffectOnArrival
{
    public ExploreBodyEffect(Mission mission, string desc = "idunnolol") : base(mission, desc)
    {
        
    }
    
    public override void ApplyEffect()
    {
        Mission.AddEffectOnReturn(new DeliverExplorationResults(Mission));
    }
}