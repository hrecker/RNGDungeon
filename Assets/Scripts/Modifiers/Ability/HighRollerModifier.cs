using System;
using Battle;

namespace Modifiers.Ability
{
    public class HighRollerModifier : Modifier, IRollValueModifier
    {
        public Tuple<int, int> ApplyRollValueMod(int playerRoll, int enemyRoll)
        {
            if (RollTrigger())
            {
                playerRoll *= 2;
                BattleController.AddPlayerModMessage("High Roller!");
            }
            return new Tuple<int, int>(playerRoll, enemyRoll);
        }
    }
}
