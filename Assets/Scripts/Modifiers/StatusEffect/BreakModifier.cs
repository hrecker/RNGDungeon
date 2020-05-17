using Battle;
using Modifiers.Generic;

namespace Modifiers.StatusEffect
{
    // Break is a status effect that lowers min roll by one
    public class BreakModifier : RollBuffModifier, IOneTimeEffectModifier
    {
        public BreakModifier(BattleActor actor) : base(0, 0)
        {
            statusEffect = Battle.StatusEffect.BREAK;
            battleEffect = RollBoundedBattleEffect.BREAK;
            // Higher intensity increases debuff of min roll
            int intensity = GetStatusEffectIntensity(actor);
            minRollDiff = -1 * intensity;
        }

        public void ApplyOneTimeEffectMod()
        {
            actor.Status().ActiveEffects.Add(Battle.StatusEffect.BREAK);
        }

        public bool CanApply()
        {
            return true;
        }

        protected override void OnDeregister()
        {
            actor.Status().ActiveEffects.Remove(Battle.StatusEffect.BREAK);
        }
    }
}
