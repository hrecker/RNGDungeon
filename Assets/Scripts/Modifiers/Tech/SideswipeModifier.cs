using Battle;
using Modifiers.StatusEffect;

namespace Modifiers.Tech
{
    // Causes 2 turn break if it hits
    public class SideswipeModifier : Modifier, IPostDamageModifier
    {
        private BattleController battleController;

        public SideswipeModifier(BattleController battleController)
        {
            this.battleController = battleController;
        }

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            BattleController.AddModMessage(actor, "Sideswipe!");
            if (rollResult.GetDamage(actor.Opponent()) > 0)
            {
                BreakModifier breakMod = new BreakModifier();
                breakMod.actor = actor.Opponent();
                breakMod.isRollBounded = true;
                breakMod.numRollsRemaining = 2;
                actor.Opponent().Status().NextRollMods.Add(breakMod);
                battleController.AddStatusMessage(actor.Opponent(), 
                    "Break: " + breakMod.numRollsRemaining + " turns");
            }
        }
    }
}
