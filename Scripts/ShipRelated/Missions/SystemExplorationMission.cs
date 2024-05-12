using System.Collections.Generic;
using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class SystemExplorationMission : Mission
{
    public SystemExplorationMission()
    {
        Name = "Discover system's planets";
        EffectsOnSuccess = new List<MissionEffect>();
        AddEffect(new DiscoverBodiesEffect());
    }
}