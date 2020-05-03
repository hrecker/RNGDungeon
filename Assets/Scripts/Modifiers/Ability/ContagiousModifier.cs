using Battle;
using Modifiers.Generic;

namespace Modifiers.Ability
{
    // Ability that gives chance to debuff opponent when hit by roll damage
    public class ContagiousModifier : Modifier, IPostDamageModifier
    {
        private int rollDebuff;
        private int rollDebuffDuration;
        private string modMessage;

        public ContagiousModifier(int rollDebuff, int rollDebuffDuration,
            string modMessage)
        {
            this.rollDebuff = rollDebuff;
            this.rollDebuffDuration = rollDebuffDuration;
            this.modMessage = modMessage;
        }

        public void ApplyPostDamageMod(RollResult initial)
        {
            // Apply debuff when hit by roll damage
            if (initial.GetRollDamage(actor) > 0 && RollTrigger())
            {
                Modifier mod = new RollBuffModifier(-rollDebuff, -rollDebuff);
                mod.isRollBounded = true;
                mod.numRollsRemaining = rollDebuffDuration;
                mod.battleEffect = RollBoundedBattleEffect.DEBUFF;
                mod.actor = actor.Opponent();
                BattleController.AddStatusMessage(actor.Opponent(), 
                    "-" + rollDebuff + " Roll: " + rollDebuffDuration + " turns");
                actor.Opponent().Status().NextRollMods.Add(mod);
                BattleController.AddModMessage(actor, modMessage);
            }
        }
    }
}
