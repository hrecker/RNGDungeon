﻿using Battle;

namespace Modifiers.StatusEffect
{
    // Chance to cause a status effect on this opponent if roll damage was dealt
    public class InflictStatusOnHitModifier : Modifier, IPostDamageModifier
    {
        private string modMessage;
        private int numStatusRolls;
        private Battle.StatusEffect status;
        private bool onlyShowModMessageOnTrigger;

        public InflictStatusOnHitModifier(Battle.StatusEffect status, 
            bool onlyShowModMessageOnTrigger, 
            string modMessage, int numStatusRolls)
        {
            this.modMessage = modMessage;
            this.numStatusRolls = numStatusRolls;
            this.status = status;
            this.onlyShowModMessageOnTrigger = onlyShowModMessageOnTrigger;
        }

        public InflictStatusOnHitModifier(Battle.StatusEffect status, 
            int numStatusRolls) : this(status, false, null, numStatusRolls) { }

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            bool trigger = false;
            if (rollResult.GetRollDamage(actor.Opponent()) > 0 && RollTrigger())
            {
                trigger = true;
                AddStatusModifier(actor.Opponent(), status, numStatusRolls);
            }
            if ((trigger || !onlyShowModMessageOnTrigger) && modMessage != null)
            {
                BattleController.AddModMessage(actor, modMessage);
            }
        }

        public static Modifier AddStatusModifier(BattleActor actor, 
            Battle.StatusEffect status, int numStatusRolls)
        {
            Modifier statusMod = status.Modifier(actor);
            statusMod.actor = actor;
            statusMod.isRollBounded = true;
            statusMod.SetStatusRollsRemaining(numStatusRolls);
            actor.Status().NextRollMods.Add(statusMod);
            BattleController.AddStatusMessage(actor,
                status.Name() + ": " + statusMod.numRollsRemaining + " rolls");
            return statusMod;
        }
    }
}
