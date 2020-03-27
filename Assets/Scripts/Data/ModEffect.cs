using System;

[Serializable]
public class ModEffect
{
    public int modPriority; // priority affects order that modifiers are applied. 1 first, then 2, etc.
    public int playerMaxHealthChange;
    public int playerMinRollChange;
    public int playerMaxRollChange;
    public float baseModTriggerChance;
}
