using System.Collections.Generic;

// Modifiers affecting the results of the roll - how much damage is done
public abstract class RollResultModifier : Modifier
{
    public abstract RollResult apply(RollResult initial);

    public override void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier(this);
    }
}
