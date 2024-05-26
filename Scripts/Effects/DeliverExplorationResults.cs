using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.Effects;

class DeliverExplorationResults : EffectOnReturn
{
    
    public DeliverExplorationResults(Mission mission, string desc = "Ship returns with scientific results") : base(mission, desc)
    {
    }

    // TODO: make it take time to analyse results
    // It should probably be via event so maybe do something with flags. Or make a timed trigger event
    // Maybe with a list of "triggers" that each have a number of turns left and the associated event to execute
    public override void ApplyEffect()
    {
        Mission.TargetBody.ExplorePlanet();
    }
}