using System;

namespace Modifiers.Ability
{
    public class HeroicModifier : Modifier, IRollGenerationModifier
    {
        private int baseMinRollBuff;
        private int baseMaxRollBuff;

        public HeroicModifier(int baseMinRollBuff, int baseMaxRollBuff)
        {
            this.baseMinRollBuff = baseMinRollBuff;
            this.baseMaxRollBuff = baseMaxRollBuff;
        }

        public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
        {
            float healthPercent = (float)PlayerStatus.Health / PlayerStatus.MaxHealth;
            // Under 25% health, the buff is doubled
            if (healthPercent <= 0.25f)
            {
                initialMinRoll += (2 * baseMinRollBuff);
                initialMaxRoll += (2 * baseMaxRollBuff);
            }
            // Under 50% health, the base buff is applied
            else if (healthPercent <= 0.5f)
            {
                initialMinRoll += baseMinRollBuff;
                initialMaxRoll += baseMaxRollBuff;
            }
            return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
        }
    }
}
