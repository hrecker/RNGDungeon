using Battle;
using Modifiers.Generic;

namespace Modifiers.Ability
{
    // Ability that gives chance to debuff opponent when hit by roll damage
    public class ContagiousModifier : Modifier, IPostDamageModifier
    {
        private int rollDebuff;
        private int rollDebuffTurns;
        private string modMessage;

        public ContagiousModifier(int rollDebuff, int rollDebuffTurns,
            string modMessage)
        {
            this.rollDebuff = rollDebuff;
            this.rollDebuffTurns = rollDebuffTurns;
            this.modMessage = modMessage;
        }

        public void ApplyPostDamageMod(RollResult initial)
        {
            // Apply debuff when hit by roll damage
            if (initial.GetRollDamage(actor) > 0 && RollTrigger())
            {
                Modifier mod = new RollBuffModifier(-rollDebuff, -rollDebuff);
                mod.isRollBounded = true;
                mod.numRollsRemaining = rollDebuffTurns;
                mod.battleEffect = RollBoundedBattleEffect.DEBUFF;
                mod.actor = actor.Opponent();
                BattleController.AddStatusMessage(actor.Opponent(), 
                    "-" + rollDebuff + " Roll: " + rollDebuffTurns + " turns");
                actor.Opponent().Status().NextRollMods.Add(mod);
                BattleController.AddModMessage(actor, modMessage);
            }
        }
    }
}
