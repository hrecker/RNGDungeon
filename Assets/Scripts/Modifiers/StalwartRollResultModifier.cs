public class StalwartRollResultModifier : Modifier, IRollResultModifier
{
    public RollResult apply(RollResult initial)
    {
        if (initial.PlayerDamage >= PlayerStatus.Health && RollTrigger())
        {
            initial.PlayerDamage--;
        }
        return initial;
    }

    public override void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier((IRollResultModifier)this);
    }
}
