using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MysteriousStatueBattleController : EnemyBattleController
{
    public Sprite phase2;
    public Sprite phase3;
    private Image battleSprite;

    public int hitsToPhase3;
    public int damageReduction;
    private int hitsTaken;
    private int currentPhase;
    private int finalPhaseMinRoll;
    private int finalPhaseMaxRoll;

    private void Start()
    {
        currentPhase = 1;
        finalPhaseMinRoll = minRoll;
        finalPhaseMaxRoll = maxRoll;
        minRoll = 1;
        maxRoll = 1;
        battleSprite = GetComponent<Image>();
    }

    private void UpdatePhase()
    {
        if (currentPhase == 3)
        {
            return;
        }

        if (hitsTaken == hitsToPhase3 - 1)
        {
            currentPhase = 2;
            battleSprite.sprite = phase2;
        }
        else if (hitsTaken >= hitsToPhase3)
        {
            currentPhase = 3;
            battleSprite.sprite = phase3;
            minRoll = finalPhaseMinRoll;
            maxRoll = finalPhaseMaxRoll;
        }
    }

    public override RollResult ApplyRollResultMods(RollResult initial)
    {
        if (currentPhase < 3)
        {
            if (initial.EnemyDamage >= 1)
            {
                initial.EnemyDamage -= damageReduction;
                if (initial.EnemyDamage < 1)
                {
                    initial.EnemyDamage = 1;
                }
            }
        }
        return initial;
    }

    public override void ApplyPostDamageEffects(RollResult rollResult)
    {
        if (rollResult.EnemyDamage > 0)
        {
            hitsTaken++;
            UpdatePhase();
        }
    }
}
