using System;

namespace Data
{
    //TODO can probably consolidate some of this when more mod properties are loaded from json
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
        POISONEDSWORD = 102,
        PUNISHINGSWORD = 103,
        CURSEDSWORD = 104,
        DEMONICSWORD = 105,
        SPECIALTYSWORD = 106,

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
        CAREPACKAGE = 210,
        HIGHSTAMINA = 211,
        CONTAGIOUS = 212,
        SNOWBALL = 213,
        SKILLED = 214,
        LIFEDRAIN = 215,
        OVERWHELMINGSPEED = 216,
        THICKSKIN = 217,
        RETALIATION = 218,
        OVERKILL = 219,
        TAUNTING = 220,
        PATIENCE = 221,
        RELIC = 222,

        // Techs: 301 - 400
        HEAVYSWING = 301,
        RAGE = 302,
        BULWARK = 303,
        SIDESWIPE = 304,
        TOPPLE = 305,
        INFECT = 306,
        OMEGASLASH = 307,
        FORTIFY = 308,
        BANDAGE = 309,
        WILDCHARGE = 310,
        PRAYER = 311,
        BIDE = 312,
        CRIT = 313,
        WILDGUESS = 314,
        WILDCURSE = 315,
        OCCULTHEALING = 316,
        FINISHINGBLOW = 317,
        ZERO = 318,
        WARMUP = 319,
        RISKYKICK = 320,

        // Items: 401 - 500
        PANACEA = 401,
        HOLYWATER = 402,
        ENERGYDRINK = 403
    }
}
