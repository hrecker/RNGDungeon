using Modifiers;
using System;
using UnityEngine;

namespace Battle.Enemies
{
    // Enemy that gets buffs at 66% and 33% health
    public class FireBugBattleController : EnemyBattleController
    {
        private Sprite phase2;
        private Sprite phase3;

        private float phase2HealthFraction = 0.67f;
        private float phase3HealthFraction = 0.34f;
        private int currentPhase;
        private int buffByPhase = 1;

        private FireBugModifier mod;

        private void Start()
        {
            currentPhase = 1;

            // Load sprites
            phase2 = GetEnemyResourceSprite("firebug2");
            phase3 = GetEnemyResourceSprite("firebug3");

            mod = new FireBugModifier(this);
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private void UpdatePhase()
        {
            if (currentPhase == 3)
            {
                return;
            }

            float healthFraction = (float)EnemyStatus.Status.Health / EnemyStatus.Status.MaxHealth;
            if (healthFraction < phase3HealthFraction)
            {
                currentPhase = 3;
                enemySprite.sprite = phase3;
                mod.battleEffect = RollBoundedBattleEffect.BUFF;
            }
            else if (healthFraction < phase2HealthFraction)
            {
                currentPhase = 2;
                enemySprite.sprite = phase2;
                mod.battleEffect = RollBoundedBattleEffect.BUFF;
            }
        }

        private int GetRollBuff()
        {
            return (currentPhase - 1) * buffByPhase;
        }

        private class FireBugModifier : Modifier, IRollGenerationModifier, IPostDamageModifier
        {
            private FireBugBattleController controller;

            public FireBugModifier(FireBugBattleController controller)
            {
                this.controller = controller;
                actor = BattleActor.ENEMY;
            }

            public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
            {
                return new Tuple<int, int>(initialMinRoll + controller.GetRollBuff(),
                    initialMaxRoll + controller.GetRollBuff());
            }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                controller.UpdatePhase();
            }
        }
    }
}
