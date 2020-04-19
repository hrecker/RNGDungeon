using System;
using Battle;

namespace Modifiers.Tech
{
    // Raise min roll but do not deal any damage
    public class BulwarkModifier : Modifier, IRollGenerationModifier, IRollResultModifier
    {
        private int minRollBuff;

        public BulwarkModifier(int minRollBuff)
        {
            this.minRollBuff = minRollBuff;
            priority = 3;
        }

        // Roll generation
        public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
        {
            // Do not allow the buff to raise the min roll above the max
            BattleController.AddModMessage(actor, "Bulwark!");
            return new Tuple<int, int>(
                Math.Min(initialMaxRoll, initialMinRoll + minRollBuff), initialMaxRoll);
        }

        // Roll result
        public RollResult ApplyRollResultMod(RollResult initial)
        {
            // Bulwark cannot damage the opponent
            initial.SetRollDamage(actor.Opponent(), 0);
            return initial;
        }
    }
}
