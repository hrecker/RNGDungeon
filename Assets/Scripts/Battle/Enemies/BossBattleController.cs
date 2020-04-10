using System;
using UnityEngine;

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
        defaultPhaseMinRoll = minRoll;
        defaultPhaseMaxRoll = maxRoll;
        currentPhase = BossPhase.DEFAULT;
        phaseRollsRemaining = 3;

        // Load sprites
        chargeSprite = GetEnemyResourceSprite("bosscharge");
        releaseSprite = GetEnemyResourceSprite("bossrelease");
        healSprite = GetEnemyResourceSprite("bossheal");
        exhaustSprite = GetEnemyResourceSprite("bossexhausted");
        defeatedSprite = GetEnemyResourceSprite("bossdefeated");
    }

    public override RollResult ApplyRollResultMods(RollResult initial)
    {
        if (currentPhase == BossPhase.HEAL)
        {
            initial.EnemyHeal += healRate;
        }
        return initial;
    }

    // Update phase after damage
    public override void ApplyPostDamageEffects(RollResult rollResult)
    {
        if (status.currentHealth > 0 && PlayerStatus.Health > 0)
        {
            // Update count of damage received while charging
            if (currentPhase == BossPhase.CHARGE)
            {
                chargeBuff += (int)Math.Max(1, rollResult.EnemyDamage);
            }

            phaseRollsRemaining--;
            if (phaseRollsRemaining == 0)
            {
                UpdatePhase();
            }
        }
        else if (status.currentHealth <= 0)
        {
            enemySprite.sprite = defeatedSprite;
        }
    }

    private void UpdatePhase()
    {
        switch (currentPhase)
        {
            case BossPhase.DEFAULT:
                // 50% chance to heal on low health
                bool healing = false;
                if (status.currentHealth / (float)status.maxHealth < 0.5f)
                {
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        currentPhase = BossPhase.HEAL;
                        enemySprite.sprite = healSprite;
                        healing = true;
                    }
                }
                // Otherwise move to charge phase
                if (!healing)
                {
                    currentPhase = BossPhase.CHARGE;
                    enemySprite.sprite = chargeSprite;
                    chargeBuff = 0;
                }
                phaseRollsRemaining = healOrChargeRolls;
                minRoll = defaultPhaseMinRoll - healOrChargeDebuff;
                maxRoll = defaultPhaseMaxRoll - healOrChargeDebuff;
                break;
            case BossPhase.CHARGE: // Charge moves to release
                currentPhase = BossPhase.RELEASE;
                phaseRollsRemaining = 1;
                enemySprite.sprite = releaseSprite;
                minRoll = defaultPhaseMinRoll + chargeBuff;
                maxRoll = defaultPhaseMaxRoll + chargeBuff;
                break;
            case BossPhase.RELEASE: // Release moves to exhaustion
                currentPhase = BossPhase.EXHAUSTED;
                phaseRollsRemaining = exhaustRolls;
                enemySprite.sprite = exhaustSprite;
                minRoll = defaultPhaseMinRoll - exhaustDebuff;
                maxRoll = defaultPhaseMaxRoll - exhaustDebuff;
                break;
            case BossPhase.HEAL: // Heal and exhaustion move back to default
            case BossPhase.EXHAUSTED:
                currentPhase = BossPhase.DEFAULT;
                phaseRollsRemaining = UnityEngine.Random.Range(
                    minDefaultPhaseRolls, maxDefaultPhaseRolls + 1);
                enemySprite.sprite = defaultSprite;
                minRoll = defaultPhaseMinRoll;
                maxRoll = defaultPhaseMaxRoll;
                break;
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
