using System.Collections.Generic;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class PlanetExplorationMission : Mission
{
    public PlanetExplorationMission()
    {
        Name = "Explore Planet";
        EffectsOnSuccess = new List<Effect>();
        AddEffect(new Effect("ExplorePlanet"));
    }
}