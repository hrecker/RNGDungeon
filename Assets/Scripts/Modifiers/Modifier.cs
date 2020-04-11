using UnityEngine;

// base class of all modifiers
public abstract class Modifier
{ 
    // Priority when applying all modifiers - lower numbers occur first, higher numbers later
    // Most modifiers should have priority 0. RollResult mods that reduce damage or have effects based
    // on damage done may need higher (later) priorities
    public int priority;
    // Whether this modifier only exists for a certain number of rolls in battle
    public bool isRollBounded;
    // Number of rolls that this modifier should exist for (if applicable)
    public int numRollsRemaining;
    // 0-1 chance that this modifier triggers (if applicable)
    public float triggerChance;

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
