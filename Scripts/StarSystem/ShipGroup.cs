using System.Collections.Generic;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

public class ShipGroup
{
    public string Name;
    public string ImagePath;
    public List<Ship> Ships;
    
    
    public ShipGroup(string name, string imagePath = "res://Assets/Icons/shipGroup.png", List<Ship> ships=null){
        Name = name;
        ImagePath = imagePath;

        if (ships!=null) Ships = new List<Ship>();
        else Ships = ships;
    }
    
    public ShipGroup(string name){
        Name = name;
        ImagePath = "res://Assets/Icons/shipGroup.png";

        Ships = new List<Ship>();
    }

    public void AddShip(Ship ship)
    {
        Ships.Add(ship);
    }
    
    
}