﻿using System;
using System.Reflection.PortableExecutable;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;

namespace GloryOfRitiria.Scripts;

public class Ship
{
    public string Name;
    public string ImagePath;
    public CelestialBody Location;
    public ShipDesign Design;
    public Cargo ShipCargo;
    public double Speed; // fraction of the speed of light, initially 1% (From Kuiper Belt to Earth (50 A.U.) in a month
    public bool Selected;
    public Route CurrentRoute;
    public ShipState State;
    

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
        Selected = false;
        Update();
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
        State = ShipState.StartingRoute;
        // Deselect ship after sending it
        Selected = false;
        game_state.SelectedShip = null;
    }

    // returns true if ship changed location
    public bool Update()
    {
        _updateState();
        _updateImage();
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

    private void _updateState()
    {
        if (CurrentRoute is null) State = Selected ? ShipState.DockedSelected : ShipState.Docked;
        else
        {   // Better somehow put this inside Route, trough status or something
            if (CurrentRoute.TotalTraveledDistance > 0) State = ShipState.InRoute;
            else State = Selected ? ShipState.StartingRouteSelected : ShipState.StartingRoute;
        }
    }

    private void _updateImage()
    {
        switch (State)
        {
            case ShipState.Docked: ImagePath = "res://Assets/Icons/ship.png"; break;
            case ShipState.DockedSelected: ImagePath = "res://Assets/Icons/selectedShip.png"; break;
            case ShipState.StartingRoute: ImagePath = "res://Assets/Icons/ShipInRoute.png"; break;
            case ShipState.StartingRouteSelected: ImagePath = "res://Assets/Icons/selectedShipInRoute.png"; break;
            case ShipState.InRoute: ImagePath = "res://Assets/Icons/ShipInRoute.png"; break;
            default: ImagePath = "res://Assets/Icons/ship.png"; break;
        }
    }
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