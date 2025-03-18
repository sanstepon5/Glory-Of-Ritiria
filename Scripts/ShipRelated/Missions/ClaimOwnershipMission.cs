using GloryOfRitiria.Scripts.Effects;

namespace GloryOfRitiria.Scripts.ShipRelated.Missions;

class ClaimOwnershipMission : Mission
{
    public ClaimOwnershipMission(): base("Claim ownership")
    {
        AddEffectOnArrival(new ClaimOwnershipEffect(this));
        Description = "Claim this system as your own.";
    }
}