using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.StarSystem;

namespace GloryOfRitiria.Scripts.Effects;

internal class ClaimOwnershipEffect : EffectOnArrival
{
	public ClaimOwnershipEffect(Mission mission, string desc = "idunnolol") : base(mission, desc)
	{
	}
	
	public override void ApplyEffect()
	{
		if (Mission.TargetBody is not Star targetStar || targetStar.GetOwnership() != StarSystemInfo.SystemOwnership.Unclaimed) 
			return;
		
		targetStar.ClaimSystem();
	}
}
