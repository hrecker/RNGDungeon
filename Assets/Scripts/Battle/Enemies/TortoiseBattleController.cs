using Modifiers.Generic;

namespace Battle.Enemies
{
    // Gets a buff after 10 turns, then another after 10 more
    public class TortoiseBattleController : EnemyBattleController
    {
        private void Start()
        {
            TortoiseModifier mod = new TortoiseModifier();
            mod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private class TortoiseModifier : RollBuffModifier
        {
            private int firstBuff = 1;
            private int secondBuff = 2;
            private int firstBuffStartRoll = 8;
            private int secondBuffStartRoll = 16;

            public TortoiseModifier() : base(0, 0) { }

            public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
            {
                currentRollGen = base.ApplyRollGenerationMod(currentRollGen);
                if (currentRollGen.CurrentRoll == firstBuffStartRoll - 1)
                {
                    minRollDiff = firstBuff;
                    maxRollDiff = firstBuff;
                    battleEffect = Modifiers.RollBoundedBattleEffect.BUFF;
                    BattleController.AddModMessage(actor, "Strengthened!");
                    BattleController.AddStatusMessage(actor, "+1 roll");
                }
                else if (currentRollGen.CurrentRoll == secondBuffStartRoll - 1)
                {
                    minRollDiff = secondBuff;
                    maxRollDiff = secondBuff;
                    BattleController.AddModMessage(actor, "Strengthened!");
                    BattleController.AddStatusMessage(actor, "+1 roll");
                }
                return currentRollGen;
            }
        }
    }
}
