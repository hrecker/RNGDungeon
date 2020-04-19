using Battle;

namespace Modifiers.StatusEffect
{
    // Poison is a status effect that causes one damage every roll
    public class PoisonModifier : Modifier, IRollResultModifier, IOneTimeEffectModifier
    {
        public PoisonModifier()
        {
            statusEffect = Battle.StatusEffect.POISON;
        }

        public void ApplyOneTimeEffectMod()
        {
            actor.Status().ActiveEffects.Add(Battle.StatusEffect.POISON);
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            initial.SetDamage(actor, initial.GetDamage(actor) + 1);
            return initial;
        }

        public bool CanApply()
        {
            return true;
        }

        protected override void OnDeregister()
        {
            actor.Status().ActiveEffects.Remove(Battle.StatusEffect.POISON);
        }
    }
}
