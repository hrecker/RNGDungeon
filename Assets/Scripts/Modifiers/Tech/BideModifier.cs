using Battle;
using Modifiers.Generic;

namespace Modifiers.Tech
{
    // Debuff for two rolls, then buff equal to roll damage received in those turns
    public class BideModifier : RollBuffModifier, IPostDamageModifier
    {
        private int rollCount;
        private int rollDamageReceived;

        public BideModifier() : base(0, 0) { }

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            if (rollCount < 3)
            {
                rollDamageReceived += rollResult.GetRollDamage(actor);
            }
            // Switch to buff after roll two
            if (rollCount == 2)
            {
                minRollDiff = rollDamageReceived;
                maxRollDiff = rollDamageReceived;
            }
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            rollCount++;
            switch (rollCount)
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
