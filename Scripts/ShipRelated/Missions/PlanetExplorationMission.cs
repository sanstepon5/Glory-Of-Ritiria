using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class PlanetExplorationMission : Mission
{
    public PlanetExplorationMission(): base("Explore Planet")
    {
        AddEffectOnArrival(new ExploreBodyEffect(this));
        Description = "Explore the planet surface to discover its scientific potential";
    }
}