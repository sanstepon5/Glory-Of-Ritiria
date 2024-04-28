using System;
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
    private List<Cargo> _shipCargo;
    public double Speed; // fraction of the speed of light, initially 1% (From Kuiper Belt to Earth (50 A.U.) in a month)
    public bool Selected;
    public ShipState State;

    private Mission _currentMission;
    
    private Route _currentRoute;

    public Ship(string name, CelestialBody body, bool inConstruction = false)
    {
        Name = name;
        if (inConstruction)
            Location = body;
        else
            SetLocation(body);
        Design = new ShipDesign();
        _shipCargo = new List<Cargo>();
        Speed = 0.01;
        Selected = false;
        State = ShipState.Docked;
    }

    public void SetSize(ShipFrameSize size)
    {
        Design.Size = size;
    }
    public void SetSize(string size)
    {
        if (Enum.TryParse(size, out ShipFrameSize frameSize))
            SetSize(frameSize);
        else
        {
            SetSize(ShipFrameSize.Medium);
            GD.Print("WARNING: Unrecognized ship frame size " + size + ", default value (Medium) is used");
        }
    }
    
    public int GetCargoCapacity()
    {
        return Design.Size switch
        {
            ShipFrameSize.Small => 2,
            ShipFrameSize.Medium => 4,
            ShipFrameSize.Large => 8,
            _ => 0
        };
    }

    /** Returns false if ship already at maximum cargo capacity, otherwise adds cargo to ship*/
    public bool AddCargo(Cargo cargo)
    {
        if (IsCargoFull())
            return false;
        _shipCargo.Add(cargo);
        return true;
    }

    /** Forces ship's cargo to be the given list */
    public void SetCargo(List<Cargo> cargoes)
    {
        _shipCargo = cargoes;
    }

    public bool IsCargoFull()
    {
        return _shipCargo.Count >= GetCargoCapacity();
    }

    public List<Cargo> GetCargo()
    {
        return _shipCargo;
    }

    public bool IsInRouteTo(CelestialBody body)
    {
        if (_currentRoute == null) return false;
        return _currentRoute.DestinationBody == body;
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
                if (_currentMission != null)
                {
                    _currentMission.ExecuteEffects();
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
        UpdateCargo();
    }

    /**Checks the cargo state and destroys it if durability reaches 0*/
    private void UpdateCargo()
    {
        var cargoToRemove = new List<Cargo>();
        foreach (var cargo in _shipCargo)
            if (cargo.Durability <= 0)
                cargoToRemove.Add(cargo);
        foreach (var cargo in cargoToRemove)
        {
            _shipCargo.Remove(cargo);
        }
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
        foreach (var cargo in _shipCargo)
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

    /** Assign the target body to the mission */
    public void SetShipMission(string missionName, CelestialBody missionTarget)
    {
        _currentMission = GetMissionByName(missionName);
        foreach (var effect in _currentMission.EffectsOnSuccess)
        {
            effect.SetBodyParam(missionTarget);
        }
    }
    
    // Returns path to a the ship icon based on ship's state and requested size
    public string GetImagePath(ShipImageSize size = ShipImageSize.Small)
    {
        string res;
        string sizeString = size switch
        {
            ShipImageSize.Small => "Small",
            ShipImageSize.Big => "Big",
            _ => "Small"
        };
        res = State switch
        {
            ShipState.Docked => "res://Assets/Icons/Ships/" + sizeString + "/ship.png",
            ShipState.DockedSelected => "res://Assets/Icons/Ships/" + sizeString + "/selectedShip.png",
            ShipState.StartingRoute => "res://Assets/Icons/Ships/" + sizeString + "/ShipInRoute.png",
            ShipState.StartingRouteSelected => "res://Assets/Icons/Ships/" + sizeString + "/selectedShipInRoute.png",
            ShipState.InRoute => "res://Assets/Icons/Ships/" + sizeString + "/ShipInRoute.png",
            _ => "res://Assets/Icons/Ships/" + sizeString + "/ship.png"
        };
        return res;
    }
}