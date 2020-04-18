using Modifiers;

namespace Battle.Enemies
{
    // Slimes regen every other roll
    public class SlimeBattleController : EnemyBattleController
    {
        private void Start()
        {
            SlimeModifier mod = new SlimeModifier();
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private class SlimeModifier : Modifier, IRollResultModifier
        {
            private int regenRate = 1;
            private bool regenRoll;

            public SlimeModifier()
            {
                actor = BattleActor.ENEMY;
            }

            public RollResult ApplyRollResultMod(RollResult initial)
            {
                if (regenRoll)
                {
                    BattleController.AddEnemyModMessage("Regen!");
                    initial.EnemyHeal += regenRate;
                }
                regenRoll = !regenRoll;
                return initial;
            }
        }
    }
}
