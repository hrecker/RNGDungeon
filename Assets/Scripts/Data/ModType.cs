using System;

namespace Data
{
    [Serializable]
    public enum ModType
    {
        // NONE represents items that don't cause any modifiers (like health potions)
        NONE = 0,

        // Generic (could be used by multiple types): 1 - 100
        BLOCK = 1,
        RECOIL = 2,
        HEALTHCHANGE = 3,

        // Weapons: 101 - 200
        WEAPON = 101,

        // Abilities: 201 - 300
        STALWART = 201,
        HIGHROLLER = 202,
        VAMPIRISM = 203,
        HEROIC = 204,
        LUCKYHORSESHOE = 205,
        PROCRASTINATION = 206,
        SPIKY = 207,
        RECOVERY = 208,
        VENOMOUS = 209,

        // Techs: 301 - 400
        HEAVYSWING = 301,
        RAGE = 302,
        BULWARK = 303,
        SIDESWIPE = 304,
        TOPPLE = 305,
        INFECT = 306
    }
}
