using Battle;
using Modifiers.Generic;

namespace Modifiers.Tech
{
    // Debuff for two turns, then buff equal to roll damage received in those turns
    public class BideModifier : RollBuffModifier, IPostDamageModifier
    {
        private int turnCount;
        private int rollDamageReceived;

        public BideModifier() : base(0, 0) { }

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            if (turnCount < 3)
            {
                rollDamageReceived += rollResult.GetRollDamage(actor);
            }
            // Switch to buff after turn two
            if (turnCount == 2)
            {
                minRollDiff = rollDamageReceived;
                maxRollDiff = rollDamageReceived;
            }
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            turnCount++;
            switch (turnCount)
            {
                case 1:
                    BattleController.AddModMessage(actor, "Bide!");
                    break;
                case 2:
                    BattleController.AddModMessage(actor, "Biding...");
                    break;
                case 3:
                    BattleController.AddModMessage(actor, "Release!");
                    break;
            }
            return base.ApplyRollGenerationMod(currentRollGen);
        }
    }
}
