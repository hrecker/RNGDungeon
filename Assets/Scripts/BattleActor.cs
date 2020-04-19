using Battle;
using System;

[Serializable]
public enum BattleActor
{
    PLAYER = 0,
    ENEMY = 1
}

public static class BattleActorExtensions
{
    public static BattleStatus Status(this BattleActor actor)
    {
        switch (actor)
        {
            case BattleActor.PLAYER:
                return PlayerStatus.Status;
            case BattleActor.ENEMY:
                return EnemyStatus.Status;
            default:
                throw new System.Exception("Invalid battle actor type");
        }
    }

    public static BattleActor Opponent(this BattleActor actor)
    {
        switch (actor)
        {
            case BattleActor.PLAYER:
                return BattleActor.ENEMY;
            case BattleActor.ENEMY:
                return BattleActor.PLAYER;
            default:
                throw new System.Exception("Invalid battle actor type");
        }
    }
}
