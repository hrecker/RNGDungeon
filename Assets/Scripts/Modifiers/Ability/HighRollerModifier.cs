using System;
using Battle;

namespace Modifiers.Ability
{
    // Chance to double the initial roll
    public class HighRollerModifier : Modifier, IRollValueModifier
    {
        public Tuple<int, int> ApplyRollValueMod(int playerRoll, int enemyRoll)
        {
            if (RollTrigger())
            {
                switch (actor)
                {
                    case BattleActor.PLAYER:
                        playerRoll *= 2;
                        break;
                    case BattleActor.ENEMY:
                        enemyRoll *= 2;
                        break;
                }
                BattleController.AddModMessage(actor, "High Roller!");
            }
            return new Tuple<int, int>(playerRoll, enemyRoll);
        }
    }
}
