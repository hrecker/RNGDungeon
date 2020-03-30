using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleController : RollGenerator
{
    private EnemyBattleStatus status;
    private Image enemySprite;
    private Enemy enemy;

    void Awake()
    {
        status = GetComponent<EnemyBattleStatus>();
        enemySprite = GetComponent<Image>();

        enemy = Cache.GetEnemy(CurrentLevel.currentEnemyName);
        status.maxHealth = enemy.maxHealth;
        status.currentHealth = status.maxHealth;
        minRoll = enemy.baseMinRoll;
        maxRoll = enemy.baseMaxRoll;

        // Load sprite
        //enemySprite.sprite = Cache.GetEnemyIcon(enemy.name);
    }
    public override int GenerateInitialRoll()
    {
        return GenerateBasicRoll(minRoll, maxRoll);
    }

    public virtual Tuple<int, int> ApplyRollValueMods(int playerInitial, int enemyInitial)
    {
        return new Tuple<int, int>(playerInitial, enemyInitial);
    }

    public virtual RollResult ApplyRollResultMods(RollResult initial)
    {
        return initial;
    }

    //TODO make a new type of modifier for this?
    public virtual void ApplyPostDamageEffects(RollResult rollResult)
    {
        // Implemented by individual enemies
    }
}
