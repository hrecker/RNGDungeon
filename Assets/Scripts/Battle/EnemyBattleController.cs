﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleController : MonoBehaviour
{
    private EnemyBattleStatus status;
    private EnemyRollGenerator rollGenerator;
    private Image enemySprite;
    private Enemy enemy;

    void Awake()
    {
        status = GetComponent<EnemyBattleStatus>();
        rollGenerator = GetComponent<EnemyRollGenerator>();
        enemySprite = GetComponent<Image>();

        enemy = Cache.GetEnemy(CurrentLevel.currentEnemyName);
        status.maxHealth = enemy.maxHealth;
        status.currentHealth = status.maxHealth;
        rollGenerator.minRoll = enemy.baseMinRoll;
        rollGenerator.maxRoll = enemy.baseMaxRoll;

        // Load sprite
        enemySprite.sprite = Cache.GetEnemyIcon(enemy.name);
    }
}
