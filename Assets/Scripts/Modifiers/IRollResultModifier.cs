// Modifiers affecting the results of the roll - how much damage is done
public interface IRollResultModifier
{
    RollResult apply(RollResult initial);
}
