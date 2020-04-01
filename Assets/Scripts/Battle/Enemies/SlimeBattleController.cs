public class SlimeBattleController : EnemyBattleController
{
    private int regenRate = 2;
    private bool regenRoll;

    public override RollResult ApplyRollResultMods(RollResult initial)
    {
        // Slimes regen every other roll
        if (regenRoll)
        {
            initial.EnemyDamage -= regenRate;
        }
        regenRoll = !regenRoll;
        return initial;
    }
}
