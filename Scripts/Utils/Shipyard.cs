﻿using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;


// Has to inherit Node to pass in signals
public partial class Shipyard : GodotObject
{
    public string ShipyardName;
    public CelestialBody Location;
    public Ship Ship;
    public SlotState State;
    public int TurnCost;
    public double CurrentProgress;

    public double BuildingEfficiency; // Base building efficiency
    public double BuildingSpeed; // Final speed after modifiers
    
    // Constructor for empty shipyard
    public Shipyard(string shipyardName, CelestialBody location, double buildingEfficiency = 1.0)
    {
        ShipyardName = shipyardName;
        Location = location;
        State = SlotState.Empty;
        Ship = null;
        CurrentProgress = 0;
        BuildingEfficiency = buildingEfficiency;
        BuildingSpeed = BuildingEfficiency;
    }
    
    // Constructor for a shipyard with a ship in construction
    // public Shipyard(string shipyardName, CelestialBody location, Ship ship, int currentProgress = 0, double buildingEfficiency = 1.0)
    // {
    //     ShipyardName = shipyardName;
    //     Location = location;
    //     State = SlotState.Busy;
    //     Ship = ship;
    //     CurrentProgress = currentProgress;
    //     TurnCost = ship.Design.Cost;
    //     BuildingEfficiency = buildingEfficiency;
    //     BuildingSpeed = BuildingEfficiency;
    // }
    
    /**Returns true if shipyard finished building*/
    public bool Update()
    {
        if (State != SlotState.Busy) return false;
        
        CurrentProgress += BuildingSpeed;
        UpdateBuildingSpeed();
        
        if (CurrentProgress < TurnCost) return false;
        
        Ship.SetLocation(Location);
        game_state.AllShips.Add(Ship);
        State = SlotState.Empty;

        return true;
    }

    public void UpdateBuildingSpeed()
    {
        BuildingSpeed = BuildingEfficiency + (BuildingEfficiency * (game_state.ScientificRes / 100));
    }

    public void StartConstruction(Ship ship)
    {
        State = SlotState.Busy;
        Ship = ship;
        CurrentProgress = 0;
        TurnCost = ship.Design.Cost;
    }

    public void CancelBuilding()
    {
        State = SlotState.Empty;
    }
    
}

public enum SlotState
{
    Busy,
    Empty,
}