public class RecoilRollResultModifer : Modifier, IRollResultModifier
{
    public RollResult apply(RollResult initial)
    {
        int damageReceived = initial.PlayerDamage;
        int recoilDamage = damageReceived > 0 ? 1 : 0;
        initial.EnemyDamage += recoilDamage;
        return initial;
    }

    public override void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier((IRollResultModifier)this);
    }
}
