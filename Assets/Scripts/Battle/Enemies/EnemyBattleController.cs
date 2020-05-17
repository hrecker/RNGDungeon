using System;
using UnityEngine;
using UnityEngine.UI;
using Levels;
using Data;
using Modifiers.Generic;

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

        public RollDamagePreventionModifier GetSingleTurnRollDamagePreventionMod(
            bool preventDamageToSelf,
            bool preventDamageToOpponent)
        {
            RollDamagePreventionModifier preventionMod = new RollDamagePreventionModifier(
                preventDamageToSelf, preventDamageToOpponent);
            preventionMod.actor = BattleActor.ENEMY;
            preventionMod.isRollBounded = true;
            preventionMod.numRollsRemaining = 1;
            return preventionMod;
        }
    }
}
