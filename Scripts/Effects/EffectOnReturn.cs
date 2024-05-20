using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.Effects;

abstract class EffectOnReturn : MissionEffect
{
    public EffectOnReturn(Mission mission, string desc) : base(mission, desc)
    {
        SetBodyParam(game_state.Pallyria);
    }
}