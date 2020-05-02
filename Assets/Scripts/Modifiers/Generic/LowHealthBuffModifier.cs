using Battle;

namespace Modifiers.Generic
{
    // Buffs when at or below 50% health. Below 25% health, the buff is doubled.
    public class LowHealthBuffModifier : Modifier, IRollGenerationModifier
    {
        private int baseMinRollBuff;
        private int baseMaxRollBuff;

        public LowHealthBuffModifier(int baseMinRollBuff, int baseMaxRollBuff)
        {
            this.baseMinRollBuff = baseMinRollBuff;
            this.baseMaxRollBuff = baseMaxRollBuff;
        }

        public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            float healthPercent = (float)Status().Health / Status().MaxHealth;
            // Under 25% health, the buff is doubled
            if (healthPercent <= 0.25f)
            {
                currentRollGen.MinRoll += (2 * baseMinRollBuff);
                currentRollGen.MaxRoll += (2 * baseMaxRollBuff);
            }
            // Under 50% health, the base buff is applied
            else if (healthPercent <= 0.5f)
            {
                currentRollGen.MinRoll += baseMinRollBuff;
                currentRollGen.MaxRoll += baseMaxRollBuff;
            }
            return currentRollGen;
        }
    }
}
