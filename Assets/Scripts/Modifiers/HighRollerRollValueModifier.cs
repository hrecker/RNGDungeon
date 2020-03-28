using System;
public class HighRollerRollValueModifier : Modifier, IRollValueModifier
{
    public Tuple<int, int> apply(int playerRoll, int enemyRoll)
    {
        if (RollTrigger())
        {
            playerRoll *= 2;
        }
        return new Tuple<int, int>(playerRoll, enemyRoll);
    }

    public override void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier((IRollValueModifier)this);
    }
}
