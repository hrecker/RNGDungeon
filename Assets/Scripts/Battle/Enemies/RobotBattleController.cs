using Modifiers.Generic;
using Modifiers;
using System;
using UnityEngine;

namespace Battle.Enemies
{
    // Initiates a self destruct attack at 25% health that goes off after 5 rolls
    public class RobotBattleController : EnemyBattleController
    {
        private Sprite selfDestructingSprite;
        private Sprite explosionSprite;

        public void Start()
        {
            selfDestructingSprite = GetEnemyResourceSprite("robotsd");
            explosionSprite = GetEnemyResourceSprite("robotexplosion");
            Modifier mod = new RobotModifier(this);
            mod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private void SetSDSprite()
        {
            enemySprite.sprite = selfDestructingSprite;
        }

        private void SetExplosionSprite()
        {
            enemySprite.sprite = explosionSprite;
        }

        private class RobotModifier : RollBuffModifier, IRollResultModifier, IPostDamageModifier
        {
            private int selfDestructInitiateHealth;
            private int selfDestructRollBuff = 5;
            private int rollsRemainingUntilSelfDestruct = 5;
            private bool isSelfDestructing;
            private RobotBattleController controller;

            public RobotModifier(RobotBattleController controller) : base(0, 0) 
            {
                this.controller = controller;
                selfDestructInitiateHealth = (int) Math.Round(0.25f * EnemyStatus.Status.MaxHealth);
            }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                if (!isSelfDestructing && EnemyStatus.Status.Health <= selfDestructInitiateHealth)
                {
                    isSelfDestructing = true;
                    AddSelfDestructStatusMessage();
                    rollsRemainingUntilSelfDestruct--;
                    controller.SetSDSprite();
                }
            }

            public RollResult ApplyRollResultMod(RollResult initial)
            {
                if (isSelfDestructing)
                {
                    AddSelfDestructStatusMessage();
                    if (rollsRemainingUntilSelfDestruct > 0)
                    {
                        rollsRemainingUntilSelfDestruct--;
                        if (rollsRemainingUntilSelfDestruct == 0)
                        {
                            minRollDiff = selfDestructRollBuff;
                            maxRollDiff = selfDestructRollBuff;
                        }
                    }
                    else
                    {
                        initial.AddNonRollDamage(BattleActor.ENEMY, 80);
                        controller.SetExplosionSprite();
                    }

                }
                return initial;
            }

            private void AddSelfDestructStatusMessage()
            {
                if (rollsRemainingUntilSelfDestruct > 0)
                {
                    BattleController.AddModMessage(BattleActor.ENEMY,
                        "Self destruct in " + rollsRemainingUntilSelfDestruct + " rolls!");
                }
                else
                {
                    BattleController.AddModMessage(BattleActor.ENEMY, "Self destruct!");
                }
            }
        }
    }
}
