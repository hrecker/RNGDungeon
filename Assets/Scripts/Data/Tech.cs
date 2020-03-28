﻿using System;

// Player battle techniques
[Serializable]
public class Tech
{
    public string name;
    public string tooltip;
    public int cooldownRolls;
    public int numRollsInEffect;
    public ModType modType;
    public ModEffect modEffect;

    public Modifier CreateTechModifier()
    {
        Modifier result = null;
        switch (modType)
        {
            case ModType.HEAVYSWING:
                result = new HeavySwingModifier();
                break;
            case ModType.RAGE:
                result = new RageModifier();
                break;
            case ModType.BULWARK:
                result = new BulwarkModifier(modEffect.playerMinRollChange);
                break;
        }
        if (result != null)
        {
            result.isRollBounded = true;
            result.numRollsRemaining = numRollsInEffect;
            result.triggerChance = modEffect.baseModTriggerChance;
        }
        return result;
    }
}