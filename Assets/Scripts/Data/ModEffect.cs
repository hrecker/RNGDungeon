using System;

namespace Data
{
    [Serializable]
    public class ModEffect //TODO make this generic to allow individual mods to parse their properties?
    {
        public int modPriority; // priority affects order that modifiers are applied. 1 first, then 2, etc.
        public int playerMaxHealthChange;
        public int playerMinRollChange;
        public int playerMaxRollChange;
        public float baseModTriggerChance;
    }
}
