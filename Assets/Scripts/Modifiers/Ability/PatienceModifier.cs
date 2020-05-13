using Battle;
using Modifiers.Generic;
using System;

namespace Modifiers.Ability
{
    // Raises roll every ten rolls into the battle, up to 3 times
    public class PatienceModifier : RollBuffModifier, IPostBattleModifier
    {
        private int rollsPerBuff = 10;
        private int maxBuffCount = 2;

        private int baseMinBuff;
        private int baseMaxBuff;

        public PatienceModifier(int baseMinBuff, int baseMaxBuff) : base(0, 0)
        {
            this.baseMinBuff = baseMinBuff;
            this.baseMaxBuff = baseMaxBuff;
        }

        public void ApplyPostBattleMod()
        {
            battleEffect = RollBoundedBattleEffect.NONE;
            minRollDiff = 0;
            maxRollDiff = 0;
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            int buffCount = 0;
            if (currentRollGen.CurrentRoll > 0)
            {
                buffCount = Math.Min(maxBuffCount, currentRollGen.CurrentRoll / rollsPerBuff);
                if (currentRollGen.CurrentRoll % rollsPerBuff == 0 &&
                    currentRollGen.CurrentRoll / rollsPerBuff <= maxBuffCount)
                {
                    BattleController.AddModMessage(actor, "Patience!");
                    BattleController.AddStatusMessage(actor, "+" + baseMaxBuff + " roll");
                    battleEffect = RollBoundedBattleEffect.BUFF;
                }
            }
            minRollDiff = (baseMinBuff * buffCount);
            maxRollDiff = (baseMaxBuff * buffCount);

            return base.ApplyRollGenerationMod(currentRollGen);
        }
    }
}
