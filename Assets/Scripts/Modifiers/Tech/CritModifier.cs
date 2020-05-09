using Battle;

namespace Modifiers.Tech
{
    // Chance to deal double damage on hit
    public class CritModifier : Modifier, IRollResultModifier
    {
        public CritModifier()
        {
            priority = 2;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (initial.GetRollDamage(actor.Opponent()) > 0 && RollTrigger())
            {
                BattleController.AddModMessage(actor, "Crit!");
                initial.AddRollDamage(actor.Opponent(),
                    initial.GetRollDamage(actor.Opponent()));
            }
            else
            {
                BattleController.AddModMessage(actor, "Crit failed!");
            }
            return initial;
        }
    }
}
