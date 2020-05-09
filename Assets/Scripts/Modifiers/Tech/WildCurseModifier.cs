using Battle;
using Modifiers.Generic;

namespace Modifiers.Tech
{
    // Chance to greatly debuff opponent
    public class WildCurseModifier : Modifier, IOneTimeEffectModifier
    {
        private int minRollDebuff;
        private int debuffDuration;

        public WildCurseModifier(int minRollDebuff, int debuffDuration)
        {
            this.minRollDebuff = minRollDebuff;
            this.debuffDuration = debuffDuration;
        }

        public void ApplyOneTimeEffectMod()
        {
            if (RollTrigger())
            {
                BattleController.AddModMessage(actor, "Wild Curse Succeeded!");
                Modifier debuff = new RollBuffModifier(minRollDebuff, 0);
                debuff.actor = actor.Opponent();
                debuff.isRollBounded = true;
                debuff.numRollsRemaining = debuffDuration;
                debuff.SetBattleEffect(RollBoundedBattleEffect.DEBUFF);
                BattleController.AddStatusMessage(actor.Opponent(),
                    minRollDebuff + " Min Roll: " + debuffDuration + " rolls");
                actor.Opponent().Status().NextRollMods.Add(debuff);
            }
            else
            {
                BattleController.AddModMessage(actor, "Wild Curse Failed!");
            }
        }

        public bool CanApply()
        {
            return true;
        }
    }
}
