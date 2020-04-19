using Battle;
using Modifiers.Generic;

namespace Modifiers.StatusEffect
{
    // Break is a status effect that lowers min roll by one
    public class BreakModifier : RollBuffModifier, IOneTimeEffectModifier
    {
        public BreakModifier() : base(-1, 0) { }

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
