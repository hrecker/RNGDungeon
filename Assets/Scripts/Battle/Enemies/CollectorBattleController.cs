using Levels;
using Modifiers.Ability;

namespace Battle.Enemies
{
    // Collector enemy that saps health and always drops a chest key
    public class CollectorBattleController : EnemyBattleController
    {
        private int healthBuffByLevel = 2;
        private int rollBuffByLevel = 1;

        private void Start()
        {
            // Reuse player vampirism ability mod, with 100% chance to trigger
            VampirismModifier mod = new VampirismModifier();
            mod.actor = BattleActor.ENEMY;
            mod.triggerChance = 1;
            EnemyStatus.Status.Mods.RegisterModifier(mod);

            // Scale roll and health by level
            int levelMultiplier = CurrentLevel.GetCurrentFloorNumber() - 1;
            int minRollBuff = levelMultiplier / 2 * rollBuffByLevel;
            int maxRollBuff = (levelMultiplier + 1) / 2 * rollBuffByLevel;
            int healthBuff = levelMultiplier * healthBuffByLevel;

            EnemyStatus.Status.BaseMinRoll = EnemyStatus.Status.BaseMinRoll + minRollBuff;
            EnemyStatus.Status.BaseMaxRoll = EnemyStatus.Status.BaseMaxRoll + maxRollBuff;
            EnemyStatus.Status.MaxHealth = EnemyStatus.Status.MaxHealth + healthBuff;
            EnemyStatus.Status.Health = EnemyStatus.Status.MaxHealth;
        }
    }
}
