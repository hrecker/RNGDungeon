using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleController : EnemyBattleController
{
    private Sprite defaultSprite;
    public Sprite chargeSprite;
    public Sprite releaseSprite;
    public Sprite healSprite;
    public Sprite exhaustSprite;
    public Sprite defeatedSprite;
    private Image bossSprite;

    public int minDefaultPhaseRolls;
    public int maxDefaultPhaseRolls;
    public int exhaustDebuff;
    public int healOrChargeDebuff;
    public int healRate;
    public int healOrChargeRolls;
    public int exhaustRolls;

    private int chargeBuff;
    private int defaultPhaseMinRoll;
    private int defaultPhaseMaxRoll;
    private BossPhase currentPhase;
    private int phaseRollsRemaining;

    void Start()
    {
        bossSprite = GetComponent<Image>();
        defaultSprite = bossSprite.sprite;
        defaultPhaseMinRoll = minRoll;
        defaultPhaseMaxRoll = maxRoll;
        currentPhase = BossPhase.DEFAULT;
        phaseRollsRemaining = 3;
    }

    public override RollResult ApplyRollResultMods(RollResult initial)
    {
        if (currentPhase == BossPhase.HEAL)
        {
            initial.EnemyDamage -= healRate;
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
            bossSprite.sprite = defeatedSprite;
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
                        bossSprite.sprite = healSprite;
                        healing = true;
                    }
                }
                // Otherwise move to charge phase
                if (!healing)
                {
                    currentPhase = BossPhase.CHARGE;
                    bossSprite.sprite = chargeSprite;
                    chargeBuff = 0;
                }
                phaseRollsRemaining = healOrChargeRolls;
                minRoll = defaultPhaseMinRoll - healOrChargeDebuff;
                maxRoll = defaultPhaseMaxRoll - healOrChargeDebuff;
                break;
            case BossPhase.CHARGE: // Charge moves to release
                currentPhase = BossPhase.RELEASE;
                phaseRollsRemaining = 1;
                bossSprite.sprite = releaseSprite;
                minRoll = defaultPhaseMinRoll + chargeBuff;
                maxRoll = defaultPhaseMaxRoll + chargeBuff;
                break;
            case BossPhase.RELEASE: // Release moves to exhaustion
                currentPhase = BossPhase.EXHAUSTED;
                phaseRollsRemaining = exhaustRolls;
                bossSprite.sprite = exhaustSprite;
                minRoll = defaultPhaseMinRoll - exhaustDebuff;
                maxRoll = defaultPhaseMaxRoll - exhaustDebuff;
                break;
            case BossPhase.HEAL: // Heal and exhaustion move back to default
            case BossPhase.EXHAUSTED:
                currentPhase = BossPhase.DEFAULT;
                phaseRollsRemaining = UnityEngine.Random.Range(
                    minDefaultPhaseRolls, maxDefaultPhaseRolls + 1);
                bossSprite.sprite = defaultSprite;
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
