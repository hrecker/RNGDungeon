using Modifiers.Ability;

namespace Battle.Enemies
{
    // Collector enemy that saps health and always drops a chest key
    public class CollectorBattleController : EnemyBattleController
    {
        private void Start()
        {
            // Reuse player vampirism ability mod, with 100% chance to trigger
            VampirismModifier mod = new VampirismModifier();
            mod.actor = BattleActor.ENEMY;
            mod.triggerChance = 1;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }
    }
}
