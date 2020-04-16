using Battle;

namespace Modifiers.Ability
{
    public class SpikyModifier : Modifier, IRollResultModifier
    {
        public SpikyModifier()
        {
            priority = 5;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (initial.PlayerDamage > 0 && RollTrigger())
            {
                initial.EnemyDamage += 1;
                BattleController.AddPlayerModMessage("Spiky!");
            }
            return initial;
        }
    }
}
