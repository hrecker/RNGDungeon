using Battle;
using Modifiers.Generic;
using System;

namespace Modifiers.Ability
{
    // Raises roll every ten rolls into the battle, up to 3 times
    public class PatienceModifier : RollBuffModifier
    {
        private int rollsPerBuff = 10;
        private int maxBuffCount = 3;

        private int baseMinBuff;
        private int baseMaxBuff;

        public PatienceModifier(int baseMinBuff, int baseMaxBuff) : base(0, 0)
        {
            this.baseMinBuff = baseMinBuff;
            this.baseMaxBuff = baseMaxBuff;
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            int buffCount = Math.Min(maxBuffCount, currentRollGen.CurrentRoll / rollsPerBuff);
            minRollDiff = (baseMinBuff * buffCount);
            maxRollDiff = (baseMaxBuff * buffCount);
            RollGeneration result = base.ApplyRollGenerationMod(currentRollGen);

            // Reset diff in case battle ends and for inventory display
            minRollDiff = 0;
            maxRollDiff = 0;

            return result;
        }
    }
}
