using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scripts;

public partial class Ship: GodotObject
{
    public string Name;
    public CelestialBody Location;
    public ShipDesign Design;
    //public Cargo ShipCargo;
    public double Speed; // fraction of the speed of light, initially 1% (From Kuiper Belt to Earth (50 A.U.) in a month
    public bool Selected;
    public ShipState State;
    
    private Route _currentRoute;

    public Ship(string name, CelestialBody body, bool inConstruction = false, string cargoName = "Default")
    {
        Name = name;
        if (inConstruction)
            Location = body;
        else
            SetLocation(body);
        Design = new ShipDesign();
        //ShipCargo = new Cargo(cargoName);
        Speed = 0.01;
        Selected = false;
        State = ShipState.Docked;
        MovementUpdate();
    }

    public bool IsInRouteTo(CelestialBody body)
    {
        if (_currentRoute == null) return false;
        if (_currentRoute.DestinationBody == body) return true;
        return false;
    }

    public bool IsInRoute()
    {
        return _currentRoute is not null;
    }

    public void ChangeSelected()
    {
        Selected = !Selected;

        if (game_state.SelectedShip == null) 
            game_state.SelectedShip = this;
        else if (game_state.SelectedShip != this)  // Deselect previous ship and select current
        {
            game_state.SelectedShip.Selected = false;
            game_state.SelectedShip = this;
        }
        else // If this ship was selected, deselect it
            game_state.SelectedShip = null; 
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
        _currentRoute = new Route(this, Location, destination);
        _updateState();
    }

    // Handles ship update on new turn (when movement possible) returns true if ship changed location
    public bool MovementUpdate()
    {
        SimpleUpdate();
        bool changeLocation;
        
        if (_currentRoute == null) return false;
        
        var oldStatus = _currentRoute.Status;
        _currentRoute.Update();
        switch (_currentRoute.Status)
        {
            case Route.RouteStatus.Arrived:
                SetLocation(_currentRoute.DestinationBody);
                _currentRoute = null;
                changeLocation = true;
                break;
            case Route.RouteStatus.InStartingSystem:
                if (_currentRoute.StartingBody is Star startingStar)
                    SetLocation(startingStar.InnerSpace);
                else
                    SetLocation(_currentRoute.StartingBody.Star.InnerSpace);
                changeLocation = true;
                break;
            case Route.RouteStatus.InDestinationSystem:
                if (oldStatus == Route.RouteStatus.InDestinationSystem) changeLocation = false;
                if (_currentRoute.DestinationBody is Star destinationStar)
                    SetLocation(destinationStar.InnerSpace);
                else
                    SetLocation(_currentRoute.DestinationBody.Star.InnerSpace);
                changeLocation = true;
                break;
            default: changeLocation = false; break;
        }
        
        // maybe overkill...
        SimpleUpdate();
        return changeLocation;
    }
    
    public void SimpleUpdate()
    {
        _updateState();
    }

    private void _updateState()
    {
        if (_currentRoute is null) State = Selected ? ShipState.DockedSelected : ShipState.Docked;
        else
        {   // Better somehow put this inside Route, trough status or something
            if (_currentRoute.TotalTraveledDistance > 0) State = ShipState.InRoute;
            else State = Selected ? ShipState.StartingRouteSelected : ShipState.StartingRoute;
        }
    }
    
    // Returns path to a the ship icon based on ship's state and requested size
    public string GetImagePath(ShipImageSize size = ShipImageSize.Small)
    {
        string res;
        string sizeString;
        switch (size)
        {
            case ShipImageSize.Small: sizeString = "Small"; break;
            case ShipImageSize.Big: sizeString = "Big"; break;
            default: sizeString = "Small"; break;
        }
        switch (State)
        {
            case ShipState.Docked: res = "res://Assets/Icons/Ships/"+sizeString+"/ship.png"; break;
            case ShipState.DockedSelected: res = "res://Assets/Icons/Ships/"+sizeString+"/selectedShip.png"; break;
            case ShipState.StartingRoute: res = "res://Assets/Icons/Ships/"+sizeString+"/ShipInRoute.png"; break;
            case ShipState.StartingRouteSelected: res = "res://Assets/Icons/Ships/"+sizeString+"/selectedShipInRoute.png"; break;
            case ShipState.InRoute: res = "res://Assets/Icons/Ships/"+sizeString+"/ShipInRoute.png"; break;
            default: res = "res://Assets/Icons/Ships/"+sizeString+"/ship.png"; break;
        }
        return res;
    }
}

public enum ShipImageSize
{
    Small,
    Big
}

public enum ShipState
{
    Docked,
    DockedSelected,
    StartingRoute,
    StartingRouteSelected,
    InRoute
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