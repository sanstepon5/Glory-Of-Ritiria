using System.Collections.Generic;
using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class PlanetExplorationMission : Mission
{
    public PlanetExplorationMission()
    {
        Name = "Explore Planet";
        EffectsOnSuccess = new List<MissionEffect>();
        AddEffect(new ExploreBodyEffect());
    }
}