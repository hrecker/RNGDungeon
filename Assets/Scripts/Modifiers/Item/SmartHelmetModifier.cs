using Battle;
using Modifiers.Generic;

namespace Modifiers.Item
{
    // Applies damage reduction under 25% health
    public class SmartHelmetModifier : RollDamageReductionModifier, IPostDamageModifier
    {
        private int fullDamageReduction;

        public SmartHelmetModifier(int damageReduction) : base(0)
        {
            fullDamageReduction = damageReduction;
        }

        public void ApplyPostDamageMod(RollResult rollResult)
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
