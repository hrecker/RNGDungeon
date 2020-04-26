using Modifiers;
using Modifiers.Generic;
using Modifiers.Ability;

namespace Battle.Enemies
{
    // Enemy that curses player with 3 turn debuff when taking damage
    public class SpiritBattleController : EnemyBattleController
    {
        private void Start()
        {
            // Reuse player contagious ability
            ContagiousModifier mod = new ContagiousModifier(1, 3, "Curse!");
            mod.actor = BattleActor.ENEMY;
            mod.triggerChance = 1;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }
    }
}
