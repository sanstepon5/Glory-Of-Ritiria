using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class PlanetExplorationMission : Mission
{
    public PlanetExplorationMission(): base("Explore Planet")
    {
        AddEffectOnArrival(new ExploreBodyEffect());
    }
}