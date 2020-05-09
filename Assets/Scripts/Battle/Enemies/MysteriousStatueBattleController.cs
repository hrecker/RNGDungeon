using UnityEngine;
using Modifiers;

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

        private MysteriousStatueModifier mod;

        private void Start()
        {
            currentPhase = 1;
            finalPhaseMinRoll = EnemyStatus.Status.BaseMinRoll;
            finalPhaseMaxRoll = EnemyStatus.Status.BaseMaxRoll;
            EnemyStatus.Status.BaseMinRoll = 1;
            EnemyStatus.Status.BaseMaxRoll = 1;

            // Load sprites
            phase2 = GetEnemyResourceSprite("mysteriousstatue2");
            phase3 = GetEnemyResourceSprite("mysteriousstatue3");

            mod = new MysteriousStatueModifier(this);
            EnemyStatus.Status.Mods.RegisterModifier(mod);
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
                mod.SetBattleEffect(RollBoundedBattleEffect.NONE);
                EnemyStatus.Status.BaseMinRoll = finalPhaseMinRoll;
                EnemyStatus.Status.BaseMaxRoll = finalPhaseMaxRoll;
            }
        }

        private class MysteriousStatueModifier : Modifier, IRollResultModifier, IPostDamageModifier
        {
            private MysteriousStatueBattleController controller;

            public MysteriousStatueModifier(MysteriousStatueBattleController controller)
            {
                this.controller = controller;
                actor = BattleActor.ENEMY;
                battleEffect = RollBoundedBattleEffect.BLOCK;
            }

            public RollResult ApplyRollResultMod(RollResult initial)
            {
                if (controller.currentPhase < 3)
                {
                    if (initial.EnemyRollDamage >= 1)
                    {
                        initial.EnemyRollDamage -= controller.damageReduction;
                        if (initial.EnemyRollDamage < 1)
                        {
                            initial.EnemyRollDamage = 1;
                        }
                    }
                }
                return initial;
            }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                if (rollResult.EnemyRollDamage > 0 && EnemyStatus.Status.Health > 0)
                {
                    controller.hitsTaken++;
                    controller.UpdatePhase();
                }
            }
        }
    }
}
