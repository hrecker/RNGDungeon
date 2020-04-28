using System;
using Battle;

namespace Modifiers.Tech
{
    // Raise roll when opponent is broken
    public class ToppleModifier : Modifier, IRollGenerationModifier
    {
        private int minRollBuff;
        private int maxRollBuff;

        public ToppleModifier(int minRollBuff, int maxRollBuff)
        {
            this.minRollBuff = minRollBuff;
            this.maxRollBuff = maxRollBuff;
        }

        public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            BattleController.AddModMessage(actor, "Topple!");
            // Only buff if opponent is broken
            if (actor.Opponent().Status().ActiveEffects.Contains(Battle.StatusEffect.BREAK))
            {
                currentRollGen.MinRoll += minRollBuff;
                currentRollGen.MaxRoll += maxRollBuff;
            }
            return currentRollGen;
        }
    }
}
