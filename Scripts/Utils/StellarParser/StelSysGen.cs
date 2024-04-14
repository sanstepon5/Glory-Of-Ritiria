using System;
using System.Collections.Generic;
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
    public static StarSystemInfo CurrentSystem;
    public static Star CurrentStar;
    public static CelestialBody CurrentBody;
    public static Ship CurrentShip;
    public static Cargo CurrentModule;
    public static Shipyard CurrentShipyard;

    public static bool IsSatellite = false;
    public static bool ShipyardBusy = false;
    public static bool InStar = true;
    

    public static void pt(StellarGeneratorPoint point)
    {
        switch (point)
        {
            case StellarGeneratorPoint.INITMAP:
            {
                break;
            }
            case StellarGeneratorPoint.ADDSYSTEM:
            {
                Data.Systems.Add(CurrentSystem);
                if (CurrentSystem.Id.Equals("detnura_aeria_system"))
                    Data.Detnura = CurrentSystem;   
                break;
            }
            
            /* Properties of Stellar System */
            case StellarGeneratorPoint.INITSYSTEM:
            {
                // CurrentText holds id
                CurrentSystem = new StarSystemInfo(CurrentText);
                break;
            }
            case StellarGeneratorPoint.SETSYSTEMNAME:
            {
                CurrentSystem.SystemName = CurrentText;
                break;
            }
            case StellarGeneratorPoint.SETSYSTEMDISTANCE:
            {
                CurrentSystem.Distance = CurrentFloat;
                break;
            }
            case StellarGeneratorPoint.SETSYSTEMANGLE:
            {
                CurrentSystem.Angle = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.SETSYSTEMPULL:
            {
                CurrentSystemPull = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.ADDSTAR:
            {
                CurrentSystem.SystemStars.Add(CurrentStar);
                break;
            }
            
            /* Properties of Stellar System */
            case StellarGeneratorPoint.INITSTAR:
            {
                CurrentStar = new Star(CurrentSystem, CurrentSystemPull);
                InStar = true;
                break;
            }
            case StellarGeneratorPoint.SETSTARNAME:
            {
                CurrentStar.Name = CurrentText;
                break;
            }
            /* Star class */
            case StellarGeneratorPoint.SETSTARCLASSOD:
            {
                CurrentStar.StarClass = StarClass.OrangeDwarf;
                break;
            }
            case StellarGeneratorPoint.SETSTARCLASSRD:
            {
                CurrentStar.StarClass = StarClass.RedDwarf;
                break;
            }
            case StellarGeneratorPoint.SETSTARCLASSYD:
            {
                CurrentStar.StarClass = StarClass.YellowDwarf;
                break;
            }
            /*Add body to star*/
            case StellarGeneratorPoint.ADDBODYTOSTAR:
            {
                CurrentStar.Bodies.Add(CurrentBody);
                break;
            }
            
            /* Properties of Celestial Body*/
            case StellarGeneratorPoint.INITBODY:
            {
                var body = new CelestialBody(CurrentText);
                body.Star = CurrentStar;

                
                if (IsSatellite)
                {
                    body.ParentBody = CurrentBody;
                    body.Satellites = new List<CelestialBody>();
                    body.IsSatellite = true;
                }

                CurrentBody = body;
                InStar = false;
                
                break;
            }
            case StellarGeneratorPoint.SETBODYNAME:
            {
                CurrentBody.Name = CurrentText;
                break;
            }
            case StellarGeneratorPoint.SETBODYTYPE:
            {
                switch (CurrentText)
                {
                    case "minor":
                    {
                        CurrentBody.BodyType = CelestialBodyType.MinorBody;
                        break;
                    }
                    default:
                    {
                        GD.Print("WARNING: Unrecognized body type " + CurrentText + ", default value is used");
                        break;
                    }
                }
                break;
            }
            /* Star class */
            case StellarGeneratorPoint.SETBODYDISTANCE:
            {
                if (CurrentInt==0)
                {
                    GD.Print("ERROR: Body " + CurrentBody.Id + " is not satellite, specifying distance is required");
                    CurrentBody.Distance = 0;
                }
                CurrentBody.Distance = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.SETBODYICON:
            {
                CurrentBody.SetImagePath(CurrentText);
                break;
            }
            case StellarGeneratorPoint.SETISNTSATELLITE:
            {
                IsSatellite = false; // All satellites are added to the parent body, exiting back to the parent body
                CurrentBody = CurrentBody.ParentBody;
                break;
            }
            
            /* Satellite properties*/
            case StellarGeneratorPoint.SETISSATELLITE:
            {
                IsSatellite = true;
                // Current body is the one that will have satellites, so all next bodies are satellites
                CurrentBody.HasSatellites = true;
                CurrentBody.Satellites = new List<CelestialBody>();
                break;
            }
            case StellarGeneratorPoint.ADDSATELLITETOPARENT:
            {
                if (CurrentBody.Distance!=0)
                {
                    GD.Print("WARNING: Body " + CurrentBody.Id + " is a satellite, distance is ignored");
                    CurrentBody.Distance = 0;
                }
                CurrentBody.ParentBody.Satellites.Add(CurrentBody);
                break;
            }
            
            /* Shipyard properties */
            case StellarGeneratorPoint.INITSHIPYARD:
            {
                CurrentShipyard = new Shipyard(CurrentText, CurrentBody);
                ShipyardBusy = false;
                break;
            }
            case StellarGeneratorPoint.SETSHIPYARDBUSY:
            {
                ShipyardBusy = true;
                break;
            }
            case StellarGeneratorPoint.ADDSHIPYARD:
            {
                CurrentBody.AddShipyard(CurrentShipyard);
                
                // Data.BodiesWithShipyards.Add(CurrentBody); Not necessary, AddShipyard already does that
                
                ShipyardBusy = false;
                break;
            }
            
            /* Ships properties*/
            case StellarGeneratorPoint.INITSHIP:
            {
                if (!ShipyardBusy)
                    CurrentShip = new Ship(CurrentText, CurrentBody);
                else
                    CurrentShip = new Ship(CurrentText, CurrentBody, true);
                break;
            }
            case StellarGeneratorPoint.SETSHIPYARDBUILDINGPROGRESS:
            {
                if (!ShipyardBusy)
                {
                    GD.Print("ERROR, ships in construction should be declared in shipyard");
                }
                CurrentShipyard.CurrentProgress = CurrentInt;
                CurrentShipyard.State = SlotState.Busy;
                
                break;
            }
            case StellarGeneratorPoint.ADDTOSHIPYARD:
            {
                CurrentShipyard.Ship = CurrentShip;
                CurrentShipyard.TurnCost = CurrentShip.Design.Cost;
                break;
            }
            case StellarGeneratorPoint.ADDSHIP:
            {
                Data.Ships.Add(CurrentShip);
                break;
            }
            
            /* Modules (cargo) properties */
            case StellarGeneratorPoint.INITMODULE:
            {
                CurrentModule = new Cargo(CurrentText);
                ShipyardBusy = false;
                break;
            }
            case StellarGeneratorPoint.SETMODULEDURABILITY:
            {
                CurrentModule.Durability = CurrentInt;
                break;
            }
            case StellarGeneratorPoint.ADDMODULE:
            {
                CurrentShip.ShipCargo.Add(CurrentModule);
                break;
            }
            
            /*Discovery status*/
            case StellarGeneratorPoint.SETSTATUSEXPLORED:
            {
                if (InStar)
                    CurrentStar.DiscoveryStatus = DiscoveryStatus.Explored;
                else
                    CurrentBody.DiscoveryStatus = DiscoveryStatus.Explored;
                break;
            }
            case StellarGeneratorPoint.SETSTATUSKNOWN:
            {
                if (InStar)
                    CurrentStar.DiscoveryStatus = DiscoveryStatus.ExistenceKnown;
                else
                    CurrentBody.DiscoveryStatus = DiscoveryStatus.ExistenceKnown;
                break;
            }
            case StellarGeneratorPoint.SETSTATUSUNDISCOVERED:
            {
                if (InStar)
                    CurrentStar.DiscoveryStatus = DiscoveryStatus.Undiscovered;
                else
                    CurrentBody.DiscoveryStatus = DiscoveryStatus.Undiscovered;
                break;
            }
            
            
            
            case StellarGeneratorPoint.ERRORVERIFICATION:
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
    public List<StarSystemInfo> Systems;
    public List<Ship> Ships;
    public List<CelestialBody> BodiesWithShipyards;
    public StarSystemInfo Detnura;

    public StelSysData()
    {
        Systems = new List<StarSystemInfo>();
        Ships = new List<Ship>();
        BodiesWithShipyards = new List<CelestialBody>();
    }
    
}