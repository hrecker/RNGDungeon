using Battle;

namespace Modifiers.Ability
{
    // Deal bonus damage when rolling a lot more than the opponent
    public class OverkillModifier : Modifier, IRollResultModifier
    {
        private int rollDiff = 5;
        private int bonusDamage = 3;

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            // If roll is at least rollDiff higher than opponent, add bonusDamage
            if (initial.GetRollValue(actor) - 
                initial.GetRollValue(actor.Opponent()) >= rollDiff)
            {
                BattleController.AddModMessage(actor, "Overkill!");
                initial.AddRollDamage(actor.Opponent(), bonusDamage);
            }
            return initial;
        }
    }
}
