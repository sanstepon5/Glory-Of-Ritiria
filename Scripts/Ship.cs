using System;
using System.Reflection.PortableExecutable;
using GloryOfRitiria.Scripts.Utils;

namespace GloryOfRitiria.Scripts;

public class Ship
{
    public string Name;
    public CelestialBody Location;
    public ShipDesign Design;
    public Cargo ShipCargo;

    public Ship(string name, CelestialBody body, bool inConstruction = false, String cargoName = "Default")
    {
        Name = name;
        if (inConstruction)
            Location = body;
        else
            SetLocation(body);
        Design = new ShipDesign();
        ShipCargo = new Cargo(cargoName);
    }

    public void SetLocation(CelestialBody location)
    {
        if (Location != null)
            Location.ShipsInOrbit.Remove(this);
        
        Location = location;
        Location.ShipsInOrbit.Add(this);
        
        //Location.ShipsInOrbit.AddShip(this);
    }
}

public class Cargo
{
    public string Name;

    public Cargo(String name)
    {
        Name = name;
    }
}