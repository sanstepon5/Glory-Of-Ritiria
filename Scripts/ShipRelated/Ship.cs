using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scripts.ShipRelated;

public partial class Ship: GodotObject
{
    public string Name;
    public CelestialBody Location;
    public ShipDesign Design;
    public List<Cargo> ShipCargo;
    public double Speed; // fraction of the speed of light, initially 1% (From Kuiper Belt to Earth (50 A.U.) in a month
    public bool Selected;
    public ShipState State;
    public Mission CurrentMission;
    
    private Route _currentRoute;

    public Ship(string name, CelestialBody body, bool inConstruction = false)
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

        ShipCargo = new List<Cargo>();
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
                if (CurrentMission != null)
                {
                    CurrentMission.ExecuteEffects();
                }
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

    public List<Mission> GetAllMissions()
    {
        var res = new List<Mission>();
        foreach (var cargo in ShipCargo)
        {
            res.AddRange(cargo.PossibleMissions);
        }
        return res;
    }

    public Mission GetMissionByName(string name)
    {
        foreach (var mission in GetAllMissions())
        {
            if (mission.Name.Equals(name)) return mission;
        }

        return null;
    }

    public void SetShipMission(string missionName, CelestialBody missionTarget)
    {
        CurrentMission = GetMissionByName(missionName);
        foreach (var effect in CurrentMission.EffectsOnSuccess)
        {
            effect.Value = missionTarget.Name;
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