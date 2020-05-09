using Battle;
using Modifiers.Generic;

namespace Modifiers.Item
{
    // Heals, cures debuffs, and buffs
    public class SuperPotionModifier : PanaceaModifier
    {
        private int healthDiff;
        private int rollDiff;
        private int buffDuration;

        public SuperPotionModifier(int healthDiff, int rollDiff, int buffDuration)
        {
            this.healthDiff = healthDiff;
            this.rollDiff = rollDiff;
            this.buffDuration = buffDuration;
        }

        public override void ApplyOneTimeEffectMod()
        {
            // Cure debuffs
            base.ApplyOneTimeEffectMod();
            // Heal
            Status().Health += healthDiff;
            // Buff
            Modifier buff = new RollBuffModifier(rollDiff, rollDiff);
            buff.isRollBounded = true;
            buff.numRollsRemaining = buffDuration;
            buff.SetBattleEffect(RollBoundedBattleEffect.BUFF);
            actor.Status().NextRollMods.Add(buff);
        }

        public override bool CanApply()
        {
            // Can always apply in battle since it gives a buff
            return BattleController.isInBattle || 
                (Status().Health < Status().MaxHealth) || 
                base.CanApply();
        }
    }
}
