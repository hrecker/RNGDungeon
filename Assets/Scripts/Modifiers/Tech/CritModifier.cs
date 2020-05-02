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
            BattleController.AddModMessage(actor, "Crit!");
            if (initial.GetRollDamage(actor.Opponent()) > 0 && RollTrigger())
            {
                initial.AddRollDamage(actor.Opponent(),
                    initial.GetRollDamage(actor.Opponent()));
            }
            return initial;
        }
    }
}
