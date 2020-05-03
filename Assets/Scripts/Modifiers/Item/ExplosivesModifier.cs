using Battle;

namespace Modifiers.Item
{
    // Deals 2 damage to player and enemy
    public class ExplosivesModifier : Modifier, IOneTimeEffectModifier
    {
        public void ApplyOneTimeEffectMod()
        {
            BattleController.AddNonRollDamage(new NonRollDamage
            {
                PlayerDamage = 2,
                EnemyDamage = 2
            });
        }

        public bool CanApply()
        {
            return true;
        }
    }
}
