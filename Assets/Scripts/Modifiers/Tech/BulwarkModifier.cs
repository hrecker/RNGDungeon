using System;
using Battle;

namespace Modifiers.Tech
{
    // Raise min roll but do not deal any damage
    public class BulwarkModifier : Modifier, IRollGenerationModifier, IRollResultModifier
    {
        private int minRollBuff;

        public BulwarkModifier(int minRollBuff)
        {
            this.minRollBuff = minRollBuff;
            priority = 3;
        }

        // Roll generation
        public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            // Do not allow the buff to raise the min roll above the max
            BattleController.AddModMessage(actor, "Bulwark!");
            currentRollGen.MinRoll = Math.Min(currentRollGen.MaxRoll, currentRollGen.MinRoll + minRollBuff);
            return currentRollGen;
        }

        // Roll result
        public RollResult ApplyRollResultMod(RollResult initial)
        {
            // Bulwark cannot damage the opponent
            initial.SetRollDamage(actor.Opponent(), 0);
            return initial;
        }
    }
}
