namespace Battle.Enemies
{
    public class SlimeBattleController : EnemyBattleController
    {
        private int regenRate = 1;
        private bool regenRoll;

        public override RollResult ApplyRollResultMods(RollResult initial)
        {
            // Slimes regen every other roll
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
