using Modifiers.Generic;

namespace Battle.Enemies
{
    // Starts with a strong buff, reduces after 3 turns, then goes away completely
    public class HareBattleController : EnemyBattleController
    {
        private void Start()
        {
            HareModifier mod = new HareModifier();
            mod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private class HareModifier : RollBuffModifier
        {
            private static int firstBuff = 2;
            private int secondBuff = 1;
            private int firstBuffEndRoll = 3;
            private int secondBuffEndRoll = 6;

            public HareModifier() : base(firstBuff, firstBuff)
            {
                battleEffect = Modifiers.RollBoundedBattleEffect.BUFF;
            }

            public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
            {
                currentRollGen = base.ApplyRollGenerationMod(currentRollGen);
                if (currentRollGen.CurrentRoll == firstBuffEndRoll)
                {
                    minRollDiff = secondBuff;
                    maxRollDiff = secondBuff;
                    BattleController.AddModMessage(actor, "Weakened!");
                    BattleController.AddStatusMessage(actor, "-1 roll");
                }
                else if (currentRollGen.CurrentRoll == secondBuffEndRoll)
                {
                    minRollDiff = 0;
                    maxRollDiff = 0;
                    battleEffect = Modifiers.RollBoundedBattleEffect.NONE;
                    BattleController.AddModMessage(actor, "Weakened!");
                    BattleController.AddStatusMessage(actor, "-1 roll");
                }
                return currentRollGen;
            }
        }
    }
}
