using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleController : RollGenerator
{
    protected EnemyBattleStatus status;
    protected Image enemySprite;
    protected Enemy enemy;

    protected const string enemySpriteResourcePath = @"Enemies/sprites/";

    protected int rollValueModPriority = 1;
    protected int rollResultModPriority = 1;
    protected int postDamageModPriority = 1;

    void Awake()
    {
        status = GetComponent<EnemyBattleStatus>();
        enemySprite = GetComponent<Image>();

        enemy = Cache.GetEnemy(CurrentLevel.currentEnemyName);
        status.maxHealth = enemy.maxHealth;
        status.currentHealth = status.maxHealth;
        minRoll = enemy.baseMinRoll;
        maxRoll = enemy.baseMaxRoll;

        // Set initial sprite
        SetSprite(GetEnemyResourceSprite(enemy.name));
    }

    protected Sprite GetEnemyResourceSprite(string spriteName)
    {
        return Resources.Load<Sprite>(enemySpriteResourcePath + spriteName);
    }

    protected void SetSprite(Sprite sprite)
    {
        enemySprite.sprite = sprite;
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

    public int GetRollValuePriority()
    {
        return rollValueModPriority;
    }

    public int GetRollResultPriority()
    {
        return rollResultModPriority;
    }

    public int GetPostDamagePriority()
    {
        return postDamageModPriority;
    }
}
