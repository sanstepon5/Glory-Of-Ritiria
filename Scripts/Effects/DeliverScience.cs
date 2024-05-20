using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.Effects;

class DeliverScience : EffectOnReturn
{
    private double _amount;
    
    public DeliverScience(Mission mission, double amount, string desc = "Ship returns with scientific results") : base(mission, desc)
    {
        _amount = amount;
    }

    // TODO: make it take time to analyse results
    // It should probably be via event so maybe do something with flags. Or make a timed trigger event
    // Maybe with a list of "triggers" that each have a number of turns left and the associated event to execute
    public override void ApplyEffect()
    {
        game_state.AddScientificRes(_amount);
    }
}