using System.Collections.Generic;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class SystemExplorationMission : Mission
{
    public SystemExplorationMission()
    {
        Name = "Discover system's planets";
        EffectsOnSuccess = new List<ShipEffect>();
        AddEffect(new ShipEffect("DiscoverSystem"));
    }
}