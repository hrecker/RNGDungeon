using UnityEngine;

public class PlayerBattleStatus : BattleStatus
{
    void Awake()
    {
        currentHealth = PlayerStatus.Health;
        maxHealth = PlayerStatus.MaxHealth;
    }

    public override void applyResult(RollResult rollResult)
    {
        currentHealth -= rollResult.PlayerDamage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        PlayerStatus.Health = currentHealth;
    }
}
