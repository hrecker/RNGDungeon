using Modifiers.StatusEffect;

namespace Battle.Enemies
{
    // Snake enemy that has 50% chance to cause 2 roll poison on hit
    public class SnakeBattleController : EnemyBattleController
    {
        private void Start()
        {
            InflictStatusOnHitModifier mod = new InflictStatusOnHitModifier(
                StatusEffect.POISON, 2);
            mod.actor = BattleActor.ENEMY;
            mod.triggerChance = 0.5f;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }
    }
}
