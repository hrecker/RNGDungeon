using System;
using Battle;

namespace Modifiers.Ability
{
    // Modifier that alternates each roll between buffing and debuffing
    public class ProcrastinationModifier : Modifier, IRollGenerationModifier
    {
        private int minRollBuff, maxRollBuff;

        public ProcrastinationModifier(int minRollBuff, int maxRollBuff)
        {
            this.minRollBuff = minRollBuff;
            this.maxRollBuff = maxRollBuff;
        }

        public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
        {
            // Don't apply when the battle hasn't started, so that the player doesn't
            // see a permanent buff in their inventory from this ability
            int currentRoll = BattleController.GetCurrentRoll();
            if (currentRoll > 0)
            {
                // Buff on odd rolls (to include the first roll), debuff on even
                if (currentRoll % 2 == 1)
                {
                    initialMinRoll += minRollBuff;
                    initialMaxRoll += maxRollBuff;
                }
                else
                {
                    initialMinRoll -= minRollBuff;
                    initialMaxRoll -= maxRollBuff;
                }
            }
            return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
        }
    }
}
