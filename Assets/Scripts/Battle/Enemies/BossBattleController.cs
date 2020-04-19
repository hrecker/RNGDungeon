using System;
using UnityEngine;
using Modifiers;

namespace Battle.Enemies
{
    // Boss enemy with multiple phases
    public class BossBattleController : EnemyBattleController
    {
        private Sprite defaultSprite;
        private Sprite chargeSprite;
        private Sprite releaseSprite;
        private Sprite healSprite;
        private Sprite exhaustSprite;
        private Sprite defeatedSprite;

        private int minDefaultPhaseRolls = 2;
        private int maxDefaultPhaseRolls = 5;
        private int exhaustDebuff = 4;
        private int healOrChargeDebuff = 2;
        private int healRate = 3;
        private int healOrChargeRolls = 2;
        private int exhaustRolls = 2;

        private int chargeBuff;
        private int defaultPhaseMinRoll;
        private int defaultPhaseMaxRoll;
        private BossPhase currentPhase;
        private int phaseRollsRemaining;

        void Start()
        {
            defaultSprite = enemySprite.sprite;
            defaultPhaseMinRoll = EnemyStatus.Status.BaseMinRoll;
            defaultPhaseMaxRoll = EnemyStatus.Status.BaseMaxRoll;
            currentPhase = BossPhase.DEFAULT;
            phaseRollsRemaining = 3;

            // Load sprites
            chargeSprite = GetEnemyResourceSprite("bosscharge");
            releaseSprite = GetEnemyResourceSprite("bossrelease");
            healSprite = GetEnemyResourceSprite("bossheal");
            exhaustSprite = GetEnemyResourceSprite("bossexhausted");
            defeatedSprite = GetEnemyResourceSprite("bossdefeated");

            BossModifier mod = new BossModifier(this);
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private void UpdatePhase()
        {
            switch (currentPhase)
            {
                case BossPhase.DEFAULT:
                    // 50% chance to heal on low health
                    bool healing = false;
                    if (EnemyStatus.Status.Health / (float)EnemyStatus.Status.MaxHealth < 0.5f)
                    {
                        if (UnityEngine.Random.value < 0.5f)
                        {
                            currentPhase = BossPhase.HEAL;
                            enemySprite.sprite = healSprite;
                            healing = true;
                            BattleController.AddModMessage(BattleActor.ENEMY, "Heal!");
                        }
                    }
                    // Otherwise move to charge phase
                    if (!healing)
                    {
                        currentPhase = BossPhase.CHARGE;
                        enemySprite.sprite = chargeSprite;
                        chargeBuff = 0;
                        BattleController.AddModMessage(BattleActor.ENEMY, "Charge!");
                    }
                    phaseRollsRemaining = healOrChargeRolls;
                    EnemyStatus.Status.BaseMinRoll = defaultPhaseMinRoll - healOrChargeDebuff;
                    EnemyStatus.Status.BaseMaxRoll = defaultPhaseMaxRoll - healOrChargeDebuff;
                    break;
                case BossPhase.CHARGE: // Charge moves to release
                    currentPhase = BossPhase.RELEASE;
                    BattleController.AddModMessage(BattleActor.ENEMY, "Release!");
                    phaseRollsRemaining = 1;
                    enemySprite.sprite = releaseSprite;
                    EnemyStatus.Status.BaseMinRoll = defaultPhaseMinRoll + chargeBuff;
                    EnemyStatus.Status.BaseMaxRoll = defaultPhaseMaxRoll + chargeBuff;
                    break;
                case BossPhase.RELEASE: // Release moves to exhaustion
                    currentPhase = BossPhase.EXHAUSTED;
                    phaseRollsRemaining = exhaustRolls;
                    enemySprite.sprite = exhaustSprite;
                    EnemyStatus.Status.BaseMinRoll = defaultPhaseMinRoll - exhaustDebuff;
                    EnemyStatus.Status.BaseMaxRoll = defaultPhaseMaxRoll - exhaustDebuff;
                    break;
                case BossPhase.HEAL: // Heal and exhaustion move back to default
                case BossPhase.EXHAUSTED:
                    currentPhase = BossPhase.DEFAULT;
                    phaseRollsRemaining = UnityEngine.Random.Range(
                        minDefaultPhaseRolls, maxDefaultPhaseRolls + 1);
                    enemySprite.sprite = defaultSprite;
                    EnemyStatus.Status.BaseMinRoll = defaultPhaseMinRoll;
                    EnemyStatus.Status.BaseMaxRoll = defaultPhaseMaxRoll;
                    break;
            }
        }

        private class BossModifier : Modifier, IRollResultModifier, IPostDamageModifier
        {
            private BossBattleController controller;

            public BossModifier(BossBattleController controller)
            {
                this.controller = controller;
                actor = BattleActor.ENEMY;
            }

            public RollResult ApplyRollResultMod(RollResult initial)
            {
                if (controller.currentPhase == BossPhase.HEAL)
                {
                    initial.EnemyHeal += controller.healRate;
                }
                return initial;
            }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                if (EnemyStatus.Status.Health > 0 && PlayerStatus.Status.Health > 0)
                {
                    // Update count of damage received while charging
                    if (controller.currentPhase == BossPhase.CHARGE)
                    {
                        controller.chargeBuff += (int)Math.Max(1, rollResult.EnemyDamage);
                    }

                    controller.phaseRollsRemaining--;
                    if (controller.phaseRollsRemaining == 0)
                    {
                        controller.UpdatePhase();
                    }
                }
                else if (EnemyStatus.Status.Health <= 0)
                {
                    controller.enemySprite.sprite = controller.defeatedSprite;
                }
            }
        }
    }

    enum BossPhase
    {
        DEFAULT,
        CHARGE,
        RELEASE,
        HEAL,
        EXHAUSTED
    }
}
