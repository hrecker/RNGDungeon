namespace Battle.Enemies
{
    public class EnemyBattleStatus : BattleStatus
    {
        public override void ApplyResult(RollResult rollResult)
        {
            currentHealth += rollResult.GetTotalEnemyHealthChange();
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}
