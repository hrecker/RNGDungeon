using Battle;
using Data;

public class EnemyStatus
{
    public static BattleStatus Status { get; set; }

    public static void Initialize(Enemy enemy)
    {
        Status = new BattleStatus(enemy.maxHealth, enemy.baseMinRoll, enemy.baseMaxRoll);
        Status.Actor = BattleActor.ENEMY;
    }
}
