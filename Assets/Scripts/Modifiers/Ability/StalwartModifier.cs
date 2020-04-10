public class StalwartModifier : Modifier, IRollResultModifier
{
    public RollResult apply(RollResult initial)
    {
        if (-initial.GetTotalPlayerHealthChange() >= PlayerStatus.Health && RollTrigger())
        {
            int damageReduction = (-initial.GetTotalPlayerHealthChange()) - PlayerStatus.Health + 1;
            initial.PlayerDamage -= damageReduction;
        }
        return initial;
    }
}
