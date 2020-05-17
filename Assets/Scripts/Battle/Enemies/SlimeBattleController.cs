using Modifiers;

namespace Battle.Enemies
{
    // Slimes regen every other roll
    public class SlimeBattleController : EnemyBattleController
    {
        private void Start()
        {
            SlimeModifier mod = new SlimeModifier(this);
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private class SlimeModifier : Modifier, IRollResultModifier, IRollGenerationModifier
        {
            private int regenRate = 1;
            private bool regenRoll;
            private EnemyBattleController controller;

            public SlimeModifier(EnemyBattleController controller)
            {
                actor = BattleActor.ENEMY;
                this.controller = controller;
            }

            public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
            {
                if (regenRoll)
                {
                    BattleController.AddModMessage(BattleActor.ENEMY, "Regen!");
                    BattleActor.ENEMY.Status().Mods.RegisterModifier(
                        controller.GetSingleTurnRollDamagePreventionMod(false, true));
                }
                return currentRollGen;
            }

            public RollResult ApplyRollResultMod(RollResult initial)
            {
                if (regenRoll)
                {
                    initial.EnemyHeal += regenRate;
                }
                regenRoll = !regenRoll;
                return initial;
            }
        }
    }
}
