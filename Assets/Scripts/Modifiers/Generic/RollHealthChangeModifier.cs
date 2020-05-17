using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modifiers.Generic
{
    // Health change that should occur during the roll, and be displayed in battle
    public class RollHealthChangeModifier : Modifier, IRollResultModifier
    {
        private int healthChange;
        private string modMessage;

        public RollHealthChangeModifier(int healthChange) :
            this(healthChange, null)
        { }

        public RollHealthChangeModifier(int healthChange, string modMessage)
        {
            this.healthChange = healthChange;
            this.modMessage = modMessage;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (modMessage != null)
            {
                BattleController.AddModMessage(actor, modMessage);
            }
            if (healthChange > 0)
            {
                initial.AddHeal(actor, healthChange);
            }
            else
            {
                initial.AddNonRollDamage(actor, healthChange);
            }
            return initial;
        }
    }
}
