using UnityEngine;

public class EnemyBattleStatus : BattleStatus
{
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public override void applyResult(RollResult rollResult)
    {
        currentHealth -= rollResult.EnemyDamage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
}
