public class CollectorBattleController : EnemyBattleController
{
    private int healthSapRate = 1;
    private int healthSapTurnsRemaining;
    private bool healthSapActive;

    public override RollResult ApplyRollResultMods(RollResult initial)
    {
        if (healthSapActive)
        {
            initial.PlayerDamage += healthSapRate;
            initial.EnemyHeal += healthSapRate;
            healthSapTurnsRemaining--;
        }
        return initial;
    }

    public override void ApplyPostDamageEffects(RollResult rollResult)
    {
        if (rollResult.PlayerDamage > 0 && !healthSapActive)
        {
            healthSapActive = true;
            healthSapTurnsRemaining = 1;
        }
        if (healthSapTurnsRemaining <= 0)
        {
            healthSapActive = false;
        }
    }
}
