using System;

public class BulwarkModifier : Modifier, IRollGenerationModifier, IRollResultModifier
{
    private int minRollBuff;

    public BulwarkModifier(int minRollBuff)
    {
        this.minRollBuff = minRollBuff;
    }

    // Roll generation
    public Tuple<int, int> apply(int initialMinRoll, int initialMaxRoll)
    {
        // Do not allow the buff to raise the min roll above the max
        BattleController.AddPlayerModMessage("Bulwark!");
        return new Tuple<int, int>(
            Math.Min(initialMaxRoll, initialMinRoll + minRollBuff), initialMaxRoll);
    }

    // Roll result
    public RollResult apply(RollResult initial)
    {
        // Bulwark cannot damage the enemy
        initial.EnemyDamage = 0;
        return initial;
    }
}
