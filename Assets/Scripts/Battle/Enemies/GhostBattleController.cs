using UnityEngine;
using Modifiers;
using Modifiers.Generic;

namespace Battle.Enemies
{
    public class GhostBattleController : EnemyBattleController
    {
        private int rollDebuff = 2;
        private bool debuffActive;
        private int debuffTurnsRemaining;

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
                    mod.isRollBounded = true;
                    mod.numRollsRemaining = 2;
                    mod.battleEffect = RollBoundedBattleEffect.DEBUFF;
                    BattleController.AddStatusMessage(BattleActor.PLAYER, "-2 Roll: 2 rolls");
                    PlayerStatus.Status.NextRollMods.Add(mod);
                    controller.debuffTurnsRemaining = 2;
                    controller.debuffActive = true;
                    BattleController.AddModMessage(BattleActor.ENEMY, "Fear!");
                }
                else if (controller.debuffActive)
                {
                    controller.debuffTurnsRemaining--;
                    if (controller.debuffTurnsRemaining <= 0)
                    {
                        controller.debuffActive = false;
                    }
                }
            }
        }
    }
}
