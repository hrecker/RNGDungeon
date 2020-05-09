using Modifiers.Generic;
using Modifiers;
using UnityEngine;

namespace Battle.Enemies
{
    // Bides and regenerates
    public class EntBattleController : EnemyBattleController
    {
        private Sprite defaultSprite;
        private Sprite bidingSprite;

        public void Start()
        {
            Modifier mod = new EntModifier(this);
            mod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
            defaultSprite = enemySprite.sprite;
            bidingSprite = GetEnemyResourceSprite("entcharge");
        }

        private void SetDefaultSprite()
        {
            enemySprite.sprite = defaultSprite;
        }

        private void SetBidingSprite()
        {
            enemySprite.sprite = bidingSprite;
        }

        private class EntModifier : RollBuffModifier, IRollResultModifier, IPostDamageModifier
        {
            private bool biding;
            private int currentBideRolls;
            private int bideDamageTaken;
            private int bideDuration = 5;
            private int firstBideRoll = 5;
            private int rollsBetweenBide = 10;
            private int regenRate = 3;

            private EntBattleController controller;

            public EntModifier(EntBattleController controller) : base(0, 0) 
            {
                this.controller = controller;
            }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                int nextRoll = rollResult.CurrentRoll + 1;
                if (biding)
                {
                    currentBideRolls++;
                    BattleController.AddModMessage(BattleActor.ENEMY,
                        "Charging... " + (bideDuration - currentBideRolls));
                    bideDamageTaken += rollResult.GetRollDamage(BattleActor.ENEMY);
                    if ((currentBideRolls + 1) == bideDuration)
                    {
                        minRollDiff = (bideDamageTaken / 2);
                        maxRollDiff = bideDamageTaken;
                    }
                }
                else if ((nextRoll - firstBideRoll) % rollsBetweenBide == 0)
                {
                    biding = true;
                    currentBideRolls = 0;
                    bideDamageTaken = 0;
                    controller.SetBidingSprite();
                    BattleController.AddModMessage(BattleActor.ENEMY,
                        "Charging... " + bideDuration);
                }
            }

            public RollResult ApplyRollResultMod(RollResult initial)
            {
                if (biding && (currentBideRolls + 1) == bideDuration &&
                    EnemyStatus.Status.Health > 0)
                {
                    BattleController.AddModMessage(BattleActor.ENEMY, "Retaliate!");
                    initial.AddHeal(BattleActor.ENEMY, regenRate);
                    biding = false;
                    controller.SetDefaultSprite();
                    minRollDiff = 0;
                    maxRollDiff = 0;
                }
                return initial;
            }
        }
    }
}