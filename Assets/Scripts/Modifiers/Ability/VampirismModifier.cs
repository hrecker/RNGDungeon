using Battle;

namespace Modifiers.Ability
{
    // Adds a chance to sap health on hit
    public class VampirismModifier : Modifier, IRollResultModifier
    {
        int healthSapRate = 1;

        public VampirismModifier()
        {
            priority = 5;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (initial.GetDamage(actor.Opponent()) > 0 && RollTrigger())
            {
                initial.SetHeal(actor, initial.GetHeal(actor) + healthSapRate);
                BattleController.AddModMessage(actor, "Vampirism!");
            }
            return initial;
        }
    }
}
