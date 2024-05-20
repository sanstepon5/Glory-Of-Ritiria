using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class PlanetaryExperiments : Mission
{
    public PlanetaryExperiments(): base("Planetary experiments")
    {
        AddEffectOnArrival(new ExploitScienceOnPlanet(this));
    }
}