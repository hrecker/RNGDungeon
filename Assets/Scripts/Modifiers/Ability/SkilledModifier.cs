using Battle;
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

        public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            if (currentRollGen.PlayerTech != null)
            {
                currentRollGen.MaxRoll += maxRollBuff;
            }
            return currentRollGen;
        }
    }
}
