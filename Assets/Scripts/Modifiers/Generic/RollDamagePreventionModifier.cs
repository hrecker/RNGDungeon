using Battle;

namespace Modifiers.Generic
{
    // Prevents roll damage for battle actors
    public class RollDamagePreventionModifier : Modifier, IRollResultModifier
    {
        private bool preventDamageToSelf;
        private bool preventDamageToOpponent;

        public RollDamagePreventionModifier(bool preventDamageToSelf, 
            bool preventDamageToOpponent)
        {
            priority = 4;
            this.preventDamageToSelf = preventDamageToSelf;
            this.preventDamageToOpponent = preventDamageToOpponent;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (preventDamageToSelf)
            {
                initial.SetRollDamage(actor, 0);
            }
            if (preventDamageToOpponent)
            {
                initial.SetRollDamage(actor.Opponent(), 0);
            }
            return initial;
        }
    }
}
