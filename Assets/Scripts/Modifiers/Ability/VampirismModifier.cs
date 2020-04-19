using Battle;

namespace Modifiers.Ability
{
    // Adds a chance to sap health on roll hit
    public class VampirismModifier : Modifier, IRollResultModifier
    {
        int healthSapRate = 1;

        public VampirismModifier()
        {
            priority = 5;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (initial.GetRollDamage(actor.Opponent()) > 0 && RollTrigger())
            {
                initial.AddHeal(actor, healthSapRate);
                BattleController.AddModMessage(actor, "Vampirism!");
            }
            return initial;
        }
    }
}
