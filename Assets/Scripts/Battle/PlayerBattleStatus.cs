using UnityEngine;

public class PlayerBattleStatus : BattleStatus
{
    public override void applyResult(RollResult rollResult)
    {
        currentHealth -= rollResult.PlayerDamage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
}
