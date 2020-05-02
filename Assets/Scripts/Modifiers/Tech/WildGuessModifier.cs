using Battle;
using Modifiers.Generic;

namespace Modifiers.Tech
{
    // Chance to greatly buff
    public class WildGuessModifier : Modifier, IOneTimeEffectModifier
    {
        private int maxRollBuff;
        private int buffDuration;

        public WildGuessModifier(int maxRollBuff, int buffDuration)
        {
            this.maxRollBuff = maxRollBuff;
            this.buffDuration = buffDuration;
        }

        public void ApplyOneTimeEffectMod()
        {
            if (RollTrigger())
            {
                BattleController.AddModMessage(actor, "Wild Guess Succeeded!");
                Modifier buff = new RollBuffModifier(0, maxRollBuff);
                buff.isRollBounded = true;
                buff.numRollsRemaining = buffDuration;
                buff.battleEffect = RollBoundedBattleEffect.BUFF;
                BattleController.AddStatusMessage(actor, 
                    "+" + maxRollBuff + " Max Roll: " + buffDuration + " rolls");
                actor.Status().NextRollMods.Add(buff);
            }
            else
            {
                BattleController.AddModMessage(actor, "Wild Guess Failed!");
            }
        }

        public bool CanApply()
        {
            return true;
        }
    }
}