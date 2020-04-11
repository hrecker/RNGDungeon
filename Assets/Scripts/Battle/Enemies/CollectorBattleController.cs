public class CollectorBattleController : EnemyBattleController
{
    private int healthSapRate = 1;

    private void Start()
    {
        // TODO constants representing what priorities should be used
        rollResultModPriority = 5;
    }

    public override RollResult ApplyRollResultMods(RollResult initial)
    {
        if (initial.PlayerDamage > 0)
        {
            initial.EnemyHeal += healthSapRate;
            BattleController.AddEnemyModMessage("Health Sap!");
        }
        return initial;
    }
}
