using System;

[Serializable]
public class ItemEffect
{
    public int playerHealthChange;
    public int playerMaxRollChange;
    public int playerMinRollChange;
    public RollBoundedEffect rollBoundedEffect;
    // If this effect includes a roll bounded effect, this
    // field indicates how many rolls it should be applied for
    public int numRollsInEffect; 
}

// This enum represents effects that stay in place for a given number of rolls,
// and then end. Examples include blocking for a certain number of rolls, or
// applying recoil damage for a certain number of rolls.
[Serializable]
public enum RollBoundedEffect
{
    NONE = 0, // No effect. Used to make 0 represent nothing in json.
    BLOCK = 1, // Block all damage, but deal no damage
    RECOIL = 2, // Apply a point of recoil damage to the opponent whenever damaged
}
