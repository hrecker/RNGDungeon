using Battle;

namespace Modifiers.StatusEffect
{
    // Poison is a status effect that causes one damage every roll
    public class PoisonModifier : Modifier, IRollResultModifier, IOneTimeEffectModifier
    {
        private int poisonDamage = 1;

        public PoisonModifier(BattleActor actor)
        {
            // Higher intensity increases poison damage
            int intensity = GetStatusEffectIntensity(actor);
            poisonDamage = intensity;
            statusEffect = Battle.StatusEffect.POISON;
            battleEffect = RollBoundedBattleEffect.POISON;
        }

        public void ApplyOneTimeEffectMod()
        {
            actor.Status().ActiveEffects.Add(Battle.StatusEffect.POISON);
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            initial.AddNonRollDamage(actor, poisonDamage);
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
