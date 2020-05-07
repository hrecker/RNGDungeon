using Modifiers.StatusEffect;
using System.Collections.Generic;

namespace Battle.Enemies
{
    // Possessed Axe enemy that has 40% chance to cause 3 roll break on hit,
    // and punishes break
    public class PossessedAxeBattleController : EnemyBattleController
    {
        private void Start()
        {
            InflictStatusOnHitModifier inflictMod = new InflictStatusOnHitModifier(
                StatusEffect.BREAK, 3);
            inflictMod.actor = BattleActor.ENEMY;
            inflictMod.triggerChance = 0.4f;
            StatusPunishingRollBuffModifier punishMod = new StatusPunishingRollBuffModifier(
                1, 1, null, new List<StatusEffect>() { Battle.StatusEffect.BREAK });
            punishMod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(inflictMod);
            EnemyStatus.Status.Mods.RegisterModifier(punishMod);
        }
    }
}
