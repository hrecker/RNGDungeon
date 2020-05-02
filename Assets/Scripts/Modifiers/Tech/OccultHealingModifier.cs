using Battle;

namespace Modifiers.Tech
{
    // Chance to heal greatly
    public class OccultHealingModifier : Modifier, IRollResultModifier
    {
        private int playerHealOnTrigger;

        public OccultHealingModifier(int playerHealOnTrigger)
        {
            this.playerHealOnTrigger = playerHealOnTrigger;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (RollTrigger())
            {
                BattleController.AddModMessage(actor, "Occult Healing Succeeded!");
                initial.AddHeal(actor, playerHealOnTrigger);
            }
            else
            {
                BattleController.AddModMessage(actor, "Occult Healing Failed!");
            }
            return initial;
        }
    }
}