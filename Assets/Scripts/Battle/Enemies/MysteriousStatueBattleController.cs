using UnityEngine;

namespace Battle.Enemies
{
    public class MysteriousStatueBattleController : EnemyBattleController
    {
        private Sprite phase2;
        private Sprite phase3;

        private int hitsToPhase3 = 3;
        private int damageReduction = 3;
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

            // Load sprites
            phase2 = GetEnemyResourceSprite("mysteriousstatue2");
            phase3 = GetEnemyResourceSprite("mysteriousstatue3");
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
                enemySprite.sprite = phase2;
            }
            else if (hitsTaken >= hitsToPhase3)
            {
                currentPhase = 3;
                enemySprite.sprite = phase3;
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
}
