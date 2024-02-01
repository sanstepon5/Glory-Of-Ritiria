using GloryOfRitiria.Scripts.Utils;

namespace GloryOfRitiria.Scripts;

public class Ship
{
    public string Name;
    public CelestialBody Location;
    public ShipDesign Design;

    public Ship(string name, CelestialBody body)
    {
        Name = name;
        Location = body;
        Design = new ShipDesign();
    }
}