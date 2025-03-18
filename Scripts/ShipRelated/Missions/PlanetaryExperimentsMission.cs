using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class PlanetaryExperimentsMission : Mission
{
    public PlanetaryExperimentsMission(): base("Planetary experiments")
    {
        AddEffectOnArrival(new ExploitScienceOnPlanet(this));
        Description = "Conduct scientific experiments on the planet surface or its orbit. This will please the scientists.";
    }
}