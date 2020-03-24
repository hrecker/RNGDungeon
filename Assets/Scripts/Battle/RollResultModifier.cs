public abstract class RollResultModifier
{
    protected bool isPlayer;
    
    public RollResultModifier(bool isPlayer)
    {
        this.isPlayer = isPlayer;
    }

    public bool IsPlayer()
    {
        return isPlayer;
    }

    public abstract RollResult apply(RollResult initial);
}
