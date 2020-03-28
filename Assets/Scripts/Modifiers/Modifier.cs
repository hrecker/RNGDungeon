using UnityEngine;

// base class of all modifiers
public abstract class Modifier
{
    public bool isRollBounded; // Whether this modifier only exists for a certain number of rolls in battle
    public int numRollsRemaining; // Number of rolls that this modifier should exist for (if applicable)
    public float triggerChance; // 0-1 chance that this modifier triggers (if applicable)

    // Randomly determine if this modifier should be triggered
    protected bool RollTrigger()
    {
        return Random.value < triggerChance;
    }

    public void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier(this);
    }
}
