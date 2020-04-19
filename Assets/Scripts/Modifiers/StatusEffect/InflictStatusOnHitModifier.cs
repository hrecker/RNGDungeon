using Battle;

namespace Modifiers.StatusEffect
{
    // Causes a status effect on this opponent if damage was dealt
    public class InflictStatusOnHitModifier : Modifier, IPostDamageModifier
    {
        private BattleController battleController;
        private string modMessage;
        private int numStatusRolls;
        private Battle.StatusEffect status;

        public InflictStatusOnHitModifier(Battle.StatusEffect status, 
            string modMessage, int numStatusRolls, BattleController battleController)
        {
            this.battleController = battleController;
            this.modMessage = modMessage;
            this.numStatusRolls = numStatusRolls;
            this.status = status;
        }

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            if (modMessage != null)
            {
                BattleController.AddModMessage(actor, modMessage);
            }
            if (rollResult.GetDamage(actor.Opponent()) > 0)
            {
                Modifier statusMod = status.Modifier();
                statusMod.actor = actor.Opponent();
                statusMod.isRollBounded = true;
                statusMod.numRollsRemaining = numStatusRolls;
                actor.Opponent().Status().NextRollMods.Add(statusMod);
                battleController.AddStatusMessage(actor.Opponent(),
                    status.Name() + ": " + statusMod.numRollsRemaining + " turns");
            }
        }
    }
}
