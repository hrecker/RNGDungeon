using System;

[Serializable]
public class Ability
{
    public string name;
    public string displayName;
    public string description;
    public ModType modType;
    public ModEffect modEffect;

    public Modifier CreateAbilityModifier()
    {
        Modifier result = null;
        switch (modType)
        {
            case ModType.HIGHROLLER:
                result = new HighRollerModifier();
                break;
            case ModType.STALWART:
                result = new StalwartModifier();
                break;
        }
        if (result != null)
        {
            result.triggerChance = modEffect.baseModTriggerChance;
            result.priority = modEffect.modPriority;
        }
        return result;
    }

    public string GetDisplayName()
    {
        return string.IsNullOrEmpty(displayName) ? name : displayName;
    }
}
