using Battle;
using Modifiers.Generic;

namespace Modifiers.Item
{
    // Applies damage reduction under 25% health
    public class SmartHelmetModifier : RollDamageReductionModifier, IPostDamageModifier
    {
        private int fullDamageReduction;

        public SmartHelmetModifier(BattleActor actor, int damageReduction) : base(0)
        {
            this.actor = actor;
            fullDamageReduction = damageReduction;
            UpdateBattleEffect();
        }

        private void UpdateBattleEffect()
        {
            if (Status().Health / (float)Status().MaxHealth < 0.25f)
            {
                battleEffect = RollBoundedBattleEffect.BLOCK;
            }
            else
            {
                battleEffect = RollBoundedBattleEffect.NONE;
            }
        }

        public override RollBoundedBattleEffect GetBattleEffect()
        {
            UpdateBattleEffect();
            return battleEffect;
        }

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            UpdateBattleEffect();
        }

        public override RollResult ApplyRollResultMod(RollResult initial)
        {
            if (Status().Health / (float)Status().MaxHealth < 0.25f)
            {
                damageReduction = fullDamageReduction;
            }
            else
            {
                damageReduction = 0;
            }

            return base.ApplyRollResultMod(initial);
        }
    }
}
