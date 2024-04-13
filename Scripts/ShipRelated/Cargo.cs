using System.Collections.Generic;
using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.ShipRelated;

public class Cargo
{
    public string Name;
    /**Number of uses left*/
    public int Durability;
    /**Cost of 0 means Cargo can only be given by event or something*/
    public int Cost;
    private string _imagePath;

    public List<Mission> PossibleMissions;


    public Cargo(string id)
    {
        Name = id;
        Durability = 1;
        Cost = 0;
        _imagePath = "res://Assets/Icons/Loadouts/spaceTelescope.png";
        PossibleMissions = new List<Mission>();


        switch (id)
        {
            case "planet_exploration_kit":
            {
                Name = "Planet exploration kit";
                Durability = 5;
                Cost = 0;
                _imagePath = "res://Assets/Icons/Loadouts/spaceTelescope.png";
                AddMission(new PlanetExplorationMission());
                
                break;
            }
            case "space_telescope":
            {
                Name = "Space Telescope";
                Durability = 1;
                Cost = 0;
                _imagePath = "res://Assets/Icons/Loadouts/spaceTelescope.png";
                AddMission(new SystemExplorationMission());
                break;
            }
        }
    }

    public Cargo(string name, int durability, int cost = 0, string imagePath = "res://Assets/Icons/Loadouts/spaceTelescope.png")
    {
        Name = name;
        Durability = durability;
        Cost = cost;
        _imagePath = imagePath;
        PossibleMissions = new List<Mission>();
    }

    public void AddMission(Mission mission)
    {
        mission.SetAssociatedCargo(this);
        PossibleMissions.Add(mission);
    }

    public void DecreaseDurability()
    {
        Durability--;
    }

    public string GetImagePath()
    {
        return _imagePath;
    }
}