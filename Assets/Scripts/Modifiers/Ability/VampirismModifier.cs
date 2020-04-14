using Battle;

namespace Modifiers.Ability
{
    public class VampirismModifier : Modifier, IRollResultModifier
    {
        int healthSapRate = 1;

        public VampirismModifier()
        {
            priority = 5;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (initial.EnemyDamage > 0 && RollTrigger())
            {
                initial.PlayerHeal += healthSapRate;
                BattleController.AddPlayerModMessage("Vampirism!");
            }
            return initial;
        }
    }
}
