using UnityEngine;

public class PlayerBattleStatus : BattleStatus
{
    void Awake()
    {
        currentHealth = PlayerStatus.Health;
        maxHealth = PlayerStatus.MaxHealth;
    }

    public override void ApplyResult(RollResult rollResult)
    {
        ApplyHealthChange(-rollResult.PlayerDamage);
    }

    public override void ApplyHealthChange(int diff)
    {
        PlayerStatus.Health += diff;
        currentHealth = PlayerStatus.Health;
        maxHealth = PlayerStatus.MaxHealth;
    }
}
