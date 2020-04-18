using System;
using UnityEngine;
using UnityEngine.UI;
using Levels;
using Data;

namespace Battle.Enemies
{
    public class EnemyBattleController : MonoBehaviour
    {
        protected Image enemySprite;
        protected Enemy enemy;

        protected const string enemySpriteResourcePath = @"Enemies/sprites/";

        void Awake()
        {
            enemySprite = GetComponent<Image>();
            enemy = Data.Cache.GetEnemy(CurrentLevel.currentEnemyName);
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
    }
}
