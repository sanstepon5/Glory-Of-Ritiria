namespace GloryOfRitiria.Scripts;

public class Ship
{
    public string Name;
    public CelestialBody Location;

    public Ship(string name, CelestialBody body)
    {
        Name = name;
        Location = body;
    }
}