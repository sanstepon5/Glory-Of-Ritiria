using System;

namespace GloryOfRitiria.Scripts;

public class Route
{
    public ShipRelated.Ship TravellingShip;
    public CelestialBody StartingBody;
    public CelestialBody DestinationBody;
    // Distance in light minutes
    public double TotalDistance; 
    public double FirstSystemDistance; 
    public double TotalTraveledDistance;
    public double FirstSystemTraveledDistance;
    public RouteStatus Status;

    public Route(ShipRelated.Ship travellingShip, CelestialBody start, CelestialBody destination)
    {
        TravellingShip = travellingShip;
        StartingBody = start;
        DestinationBody = destination;
        
        _setupDistances(start, destination);
        
        TotalTraveledDistance = 0;
        FirstSystemTraveledDistance = 0;
        Status = RouteStatus.InStartingSystem;
    }

    private void _setupDistances(CelestialBody start, CelestialBody destination)
    {
        // Compute distances to travel
        if (start.Star == destination.Star) // Travelling within the same system
        {   
            FirstSystemDistance = Math.Abs(destination.Distance - start.Distance);
            TotalDistance = FirstSystemDistance;
        }
        else // Need to leave starting star system
        {   // Compute distance within the starting system
            if (start.Distance >= start.Star.OuterSpaceDistance)    // If farther then boundaries of the system
                FirstSystemDistance = 0;
            else 
                FirstSystemDistance = start.Star.OuterSpaceDistance - start.Distance;
            
            // Compute distance within the destination system
            double secondSystemDistance;
            if (destination.Distance >= destination.Star.OuterSpaceDistance) // If farther then boundaries of the system
                secondSystemDistance = 0;   // Can jump directly to destination
            else 
                secondSystemDistance = destination.Star.OuterSpaceDistance - destination.Distance;
            TotalDistance = FirstSystemDistance + secondSystemDistance;
        }
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