using System;
using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;

public class StelSysGen
{
    public static string CurrentText;
    public static int CurrentInt;
    public static float CurrentFloat;
    
    // A property of system that is the same for all stars of the system
    // It should be in StarSystemInfo, but I'm not refactoring it now...
    public static int CurrentSystemPull;
    
    public static StelSysData Data = new ();
    public static StarSystem.StarSystemInfo CurrentSystem;
    public static Star CurrentStar;
    public static CelestialBody CurrentBody;
    // Allows to trace back to the "highest" parent
    public static Stack<CelestialBody> ParentBodies;
    public static Ship CurrentShip;
    public static Cargo CurrentModule;
    public static ShipRelated.Shipyard CurrentShipyard;

    public static bool IsSatellite = false;
    public static bool ShipyardBusy = false;
    public static bool InStar = true;
    

    public static void pt(StellarGeneratorPoint point)
    {
        switch (point)
        {
            case StellarGeneratorPoint.Initmap:
            {
                break;
            }
            case StellarGeneratorPoint.Addsystem:
            {
                Data.Systems.Add(CurrentSystem);
                if (CurrentSystem.Id.Equals("detnura_aeria_system"))
                    Data.Detnura = CurrentSystem;   
                break;
            }
            
            /* Properties of Stellar System */
            case StellarGeneratorPoint.Initsystem:
            {
                // CurrentText holds id
                CurrentSystem = new StarSystem.StarSystemInfo(CurrentText);
                break;
            }
            case StellarGeneratorPoint.Setsystemname:
            {
                CurrentSystem.SystemName = CurrentText;
                break;
            }
            case StellarGeneratorPoint.Setsystemdistance:
            {
                CurrentSystem.Distance = CurrentFloat;
                break;
            }
            case StellarGeneratorPoint.Setsystemangle:
            {
                CurrentSystem.Angle = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.Setsystempull:
            {
                CurrentSystemPull = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.Addstar:
            {
                CurrentSystem.SystemStars.Add(CurrentStar);
                break;
            }
            
            /* Properties of Stellar System */
            case StellarGeneratorPoint.Initstar:
            {
                CurrentStar = new Star(CurrentSystem, CurrentSystemPull);
                ParentBodies = new Stack<CelestialBody>();
                InStar = true;
                break;
            }
            case StellarGeneratorPoint.Setstarname:
            {
                CurrentStar.Name = CurrentText;
                break;
            }
            /* Star class */
            case StellarGeneratorPoint.Setstarclassod:
            {
                CurrentStar.StarClass = StarClass.OrangeDwarf;
                break;
            }
            case StellarGeneratorPoint.Setstarclassrd:
            {
                CurrentStar.StarClass = StarClass.RedDwarf;
                break;
            }
            case StellarGeneratorPoint.Setstarclassyd:
            {
                CurrentStar.StarClass = StarClass.YellowDwarf;
                break;
            }
            /*Add body to star*/
            case StellarGeneratorPoint.Addbodytostar:
            {
                CurrentStar.Bodies.Add(CurrentBody);
                if (CurrentBody.Id.Equals("pallyria"))
                    game_state.Pallyria = CurrentBody;
                break;
            }
            
            /* Properties of Celestial Body*/
            case StellarGeneratorPoint.Initbody:
            {
                var body = new CelestialBody(CurrentText);
                body.Star = CurrentStar;
                
                
                if (IsSatellite)
                {
                    body.ParentBody = CurrentBody;
                    body.IsSatellite = true;
                }

                CurrentBody = body;
                InStar = false;
                
                break;
            }
            case StellarGeneratorPoint.Setbodyname:
            {
                CurrentBody.Name = CurrentText;
                break;
            }
            case StellarGeneratorPoint.Setbodytype:
            {
                if (Enum.TryParse(CurrentText, out CelestialBodyType type))
                    CurrentBody.BodyType = type;
                else
                {
                    CurrentBody.BodyType = CelestialBodyType.DefaultPlanet;
                    GD.Print("WARNING: Unrecognized body type " + CurrentText + ", default value is used");
                }
                break;
            }
            /* Star class */
            case StellarGeneratorPoint.Setbodydistance:
            {
                if (CurrentInt==0)
                {
                    GD.Print("ERROR: Body " + CurrentBody.Id + " is not satellite, specifying distance is required");
                    CurrentBody.Distance = 0;
                }
                CurrentBody.Distance = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.Setbodyicon:
            {
                CurrentBody.SetImagePath(CurrentText);
                break;
            }
            case StellarGeneratorPoint.Setisntsatellite:
            {
                IsSatellite = false; // All satellites are added to the parent body, exiting back to the parent body
                CurrentBody = ParentBodies.Pop();
                
                break;
            }
            
            /* Satellite properties*/
            case StellarGeneratorPoint.Setissatellite:
            {
                IsSatellite = true;
                // Current body is the one that will have satellites, so all next bodies are satellites
                CurrentBody.HasSatellites = true;
                CurrentBody.Satellites = new List<CelestialBody>();
                ParentBodies.Push(CurrentBody);
                break;
            }
            case StellarGeneratorPoint.Addsatellitetoparent:
            {
                if (CurrentBody.Distance!=0)
                {
                    GD.Print("WARNING: Body " + CurrentBody.Id + " is a satellite, distance is ignored");
                    CurrentBody.Distance = 0;
                }
                // Parent body may have more satellites after this one
                ParentBodies.Peek().AddSatellite(CurrentBody);
                break;
            }
            
            /* Shipyard properties */
            case StellarGeneratorPoint.Initshipyard:
            {
                CurrentShipyard = new ShipRelated.Shipyard(CurrentText, CurrentBody);
                ShipyardBusy = false;
                break;
            }
            case StellarGeneratorPoint.Setshipyardbusy:
            {
                ShipyardBusy = true;
                break;
            }
            case StellarGeneratorPoint.Addshipyard:
            {
                CurrentBody.AddShipyard(CurrentShipyard);
                
                // Data.BodiesWithShipyards.Add(CurrentBody); Not necessary, AddShipyard already does that
                
                ShipyardBusy = false;
                break;
            }
            
            /* Ships properties*/
            case StellarGeneratorPoint.Initship:
            {
                if (!ShipyardBusy)
                    CurrentShip = new Ship(CurrentText, CurrentBody);
                else
                    CurrentShip = new Ship(CurrentText, CurrentBody, true);
                break;
            }
            case StellarGeneratorPoint.Setshipsize:
            {
                CurrentShip.SetSize(CurrentText);
                break;
            }
            case StellarGeneratorPoint.Setshipyardbuildingprogress:
            {
                if (!ShipyardBusy)
                {
                    GD.Print("ERROR, ships in construction should be declared in shipyard");
                }
                CurrentShipyard.CurrentProgress = CurrentInt;
                CurrentShipyard.State = SlotState.Busy;
                
                break;
            }
            case StellarGeneratorPoint.Addtoshipyard:
            {
                CurrentShipyard.Ship = CurrentShip;
                CurrentShipyard.TurnCost = CurrentShip.Design.Cost;
                break;
            }
            case StellarGeneratorPoint.Addship:
            {
                Data.Ships.Add(CurrentShip);
                break;
            }
            
            /* Modules (cargo) properties */
            case StellarGeneratorPoint.Initmodule:
            {
                CurrentModule = new Cargo(CurrentText);
                ShipyardBusy = false;
                break;
            }
            case StellarGeneratorPoint.Setmoduledurability:
            {
                CurrentModule.Durability = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.Addmodule:
            {
                if (!CurrentShip.AddCargo(CurrentModule))
                {
                    GD.PrintErr("Ship " + CurrentShip.Name + " cargo capacity is at maximum (" + 
                                CurrentShip.GetCargoCapacity()+"), module " + CurrentModule.Name + " is ignored");
                }
                break;
            }
            
            /*Discovery status*/
            case StellarGeneratorPoint.Setstatusexplored:
            {
                if (InStar)
                    CurrentStar.DiscoveryStatus = DiscoveryStatus.Explored;
                else
                    CurrentBody.DiscoveryStatus = DiscoveryStatus.Explored;
                break;
            }
            case StellarGeneratorPoint.Setstatusknown:
            {
                if (InStar)
                    CurrentStar.DiscoveryStatus = DiscoveryStatus.ExistenceKnown;
                else
                    CurrentBody.DiscoveryStatus = DiscoveryStatus.ExistenceKnown;
                break;
            }
            case StellarGeneratorPoint.Setstatusundiscovered:
            {
                if (InStar)
                    CurrentStar.DiscoveryStatus = DiscoveryStatus.Undiscovered;
                else
                    CurrentBody.DiscoveryStatus = DiscoveryStatus.Undiscovered;
                break;
            }
            
            
            
            case StellarGeneratorPoint.Errorverification:
            {
                // Null checking
                if (Data.Detnura == null)
                    GD.Print("ERROR, No Detnura system found in file");
                break;
            }
        }
    }


    /**This function has to be called after all necessary information has been extracted to game_state.
     * If not done, memory will be occupied by redundant information about the initial state of stellar systems.
     * Returns a struct containing all parsed information.
     */
    public static StelSysData UnloadData()
    {
        // Saving parsing result in a non static variable to return
        // In theory it simply holds references to the static context, so memory wise it should be ok
        var res = new StelSysData();
        res.Systems = Data.Systems;
        res.Ships = Data.Ships;
        res.BodiesWithShipyards = Data.BodiesWithShipyards;
        res.Detnura = Data.Detnura;
        
        // Cleaning static data
        Data = null;

        CurrentText = null;
        CurrentInt = 0;
        CurrentFloat = 0;
        CurrentSystem = null;
        CurrentStar = null;

        return res;
    }
}

/**Utility class that stores parsed data*/
public class StelSysData
{
    public List<StarSystem.StarSystemInfo> Systems;
    public List<Ship> Ships;
    public List<CelestialBody> BodiesWithShipyards;
    public StarSystem.StarSystemInfo Detnura;

    public StelSysData()
    {
        Systems = new List<StarSystem.StarSystemInfo>();
        Ships = new List<Ship>();
        BodiesWithShipyards = new List<CelestialBody>();
    }
    
}