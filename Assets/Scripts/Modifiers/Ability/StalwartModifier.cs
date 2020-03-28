public class StalwartModifier : Modifier, IRollResultModifier
{
    public RollResult apply(RollResult initial)
    {
        if (initial.PlayerDamage >= PlayerStatus.Health && RollTrigger())
        {
            initial.PlayerDamage--;
        }
        return initial;
    }
}
