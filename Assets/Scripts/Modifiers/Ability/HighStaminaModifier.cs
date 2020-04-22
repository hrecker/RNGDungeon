using System;
using UnityEngine;

namespace Modifiers.Ability
{
    // Mod that reduces tech cooldowns by 2
    public class HighStaminaModifier : Modifier, ITechModifier
    {
        public int ApplyTechCooldownModifier(int startingCooldown)
        {
            return Math.Max(0, startingCooldown - 2);
        }
    }
}
