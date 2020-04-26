using System;

namespace Modifiers.Ability
{
    // Ability that buffs max roll for all techs
    public class SkilledModifier : Modifier, IRollGenerationModifier
    {
        private int maxRollBuff;

        public SkilledModifier(int maxRollBuff)
        {
            this.maxRollBuff = maxRollBuff;
        }

        public Tuple<int, int> ApplyRollGenerationMod(Data.Tech tech, int initialMinRoll, int initialMaxRoll)
        {
            if (tech != null)
            {
                initialMaxRoll += maxRollBuff;
            }
            return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
        }
    }
}
