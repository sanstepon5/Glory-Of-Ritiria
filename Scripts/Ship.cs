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
    public double Speed; // fraction of the speed of light, initially 1% (From Kuiper Belt to Earth (50 A.U.) in a month
    public Route CurrentRoute;

    public Ship(string name, CelestialBody body, bool inConstruction = false, string cargoName = "Default")
    {
        Name = name;
        if (inConstruction)
            Location = body;
        else
            SetLocation(body);
        Design = new ShipDesign();
        ShipCargo = new Cargo(cargoName);
        Speed = 0.01;
    }

    public void SetLocation(CelestialBody location)
    {
        if (Location != null)
            Location.ShipsInOrbit.Remove(this);
        
        Location = location;
        Location.ShipsInOrbit.Add(this);
    }

    public void StartRoute(CelestialBody destination)
    {
        CurrentRoute = new Route(this, Location, destination);
    }

    // returns true if ship changed location
    public bool Update()
    {
        if (CurrentRoute == null) return false;
        var oldStatus = CurrentRoute.Status;
        CurrentRoute.Update();
        switch (CurrentRoute.Status)
        {
            case Route.RouteStatus.Arrived:
                if (oldStatus == Route.RouteStatus.Arrived) return false;
                SetLocation(CurrentRoute.DestinationBody);
                CurrentRoute = null;
                return true;
            case Route.RouteStatus.InStartingSystem:
                //if (oldStatus == Route.RouteStatus.InStartingSystem) return false;
                SetLocation(CurrentRoute.StartingBody.Star.InnerSpace);
                return true;
            case Route.RouteStatus.InDestinationSystem:
                if (oldStatus == Route.RouteStatus.InDestinationSystem) return false;
                SetLocation(CurrentRoute.DestinationBody.Star.InnerSpace);
                return true;
            default: return false;
        }
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

public class Route
{
    public Ship TravellingShip;
    public CelestialBody StartingBody;
    public CelestialBody DestinationBody;
    // Distance in light minutes
    public double TotalDistance; 
    public double FirstSystemDistance; 
    public double TotalTraveledDistance;
    public double FirstSystemTraveledDistance;
    public RouteStatus Status;

    public Route(Ship travellingShip, CelestialBody start, CelestialBody destination)
    {
        TravellingShip = travellingShip;
        StartingBody = start;
        DestinationBody = destination;

        FirstSystemDistance = StartingBody.Star.OuterSpaceDistance - StartingBody.Distance;
        double secondSystemDistance = 0;
        if (StartingBody.Star != DestinationBody.Star)
        {
            secondSystemDistance = DestinationBody.Star.OuterSpaceDistance - DestinationBody.Distance;
        } 
        
        TotalDistance = FirstSystemDistance + secondSystemDistance;
        
        TotalTraveledDistance = 0;
        FirstSystemTraveledDistance = 0;
        Status = RouteStatus.InStartingSystem;
    }

    public void Update()
    {
        var traveledDistance = ComputeTraveledDistanceInTurn(TravellingShip.Speed);
        if (Status == RouteStatus.InStartingSystem)
        {
            FirstSystemTraveledDistance += traveledDistance;
        }
        
        TotalTraveledDistance += traveledDistance;
        
        if (TotalTraveledDistance >= TotalDistance)
        {
            Status = RouteStatus.Arrived;
        }
        else
        {
            if (FirstSystemTraveledDistance >= FirstSystemDistance)
            {
                Status = RouteStatus.InDestinationSystem;
            }
        }
    }
    
    

    // Compute how many light hours will be traveled in a single turn (one month)
    // With a speed of 0.01c 7.2*60 light minutes will be travelled in a month
    // 50 A.U ~= 7.2 light hours = 432 light minutes
    public static double ComputeTraveledDistanceInTurn(double speed)
    {
        return 30 * 24 * 60 * speed;
    }

    public enum RouteStatus
    {
        InStartingSystem,
        InDestinationSystem,
        Arrived
    }
    
}