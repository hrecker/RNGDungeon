using System;
using Battle;

namespace Modifiers.Tech
{
    // Roll a zero
    public class ZeroModifier : Modifier, IRollValueModifier
    {
        public ZeroModifier()
        {
            priority = 10;
        }

        public Tuple<int, int> ApplyRollValueMod(int playerRoll, int enemyRoll)
        {
            BattleController.AddModMessage(actor, "Zero!");
            if (actor == BattleActor.PLAYER)
            {
                playerRoll = 0;
            }
            else
            {
                enemyRoll = 0;
            }
            return new Tuple<int, int>(playerRoll, enemyRoll);
        }
    }
}
