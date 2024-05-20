using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class SystemExplorationMission : Mission
{
    public SystemExplorationMission(): base("Discover system's planets")
    {
        AddEffectOnArrival(new DiscoverBodiesEffect());
    }
}