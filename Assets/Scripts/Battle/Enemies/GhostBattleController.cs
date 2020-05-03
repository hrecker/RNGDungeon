using UnityEngine;
using Modifiers;
using Modifiers.Generic;

namespace Battle.Enemies
{
    public class GhostBattleController : EnemyBattleController
    {
        private int rollDebuff = 2;
        private bool debuffActive;
        private int debuffRollsRemaining;

        private void Start()
        {
            GhostModifier mod = new GhostModifier(this);
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private class GhostModifier : Modifier, IPostDamageModifier
        {
            GhostBattleController controller;

            public GhostModifier(GhostBattleController controller)
            {
                this.controller = controller;
                actor = BattleActor.ENEMY;
            }

            public void ApplyPostDamageMod(RollResult initial)
            {
                if (initial.PlayerRollDamage > 0 && !controller.debuffActive)
                {
                    Modifier mod = new RollBuffModifier(-controller.rollDebuff, -controller.rollDebuff);
                    mod.actor = BattleActor.PLAYER;
                    mod.isRollBounded = true;
                    mod.numRollsRemaining = 2;
                    mod.battleEffect = RollBoundedBattleEffect.DEBUFF;
                    BattleController.AddStatusMessage(BattleActor.PLAYER, "-2 Roll: 2 rolls");
                    PlayerStatus.Status.NextRollMods.Add(mod);
                    controller.debuffRollsRemaining = 2;
                    controller.debuffActive = true;
                    BattleController.AddModMessage(BattleActor.ENEMY, "Fear!");
                }
                else if (controller.debuffActive)
                {
                    controller.debuffRollsRemaining--;
                    if (controller.debuffRollsRemaining <= 0)
                    {
                        controller.debuffActive = false;
                    }
                }
            }
        }
    }
}
