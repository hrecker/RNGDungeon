using Modifiers.StatusEffect;
using Modifiers.Generic;
using Modifiers;
using UnityEngine;

namespace Battle.Enemies
{
    // Puts a debuff or status on the player every 3 rolls
    public class WitchBattleController : EnemyBattleController
    {
        private int rollDebuff = 2;
        private int debuffDuration = 3;

        public void Start()
        {
            Modifier mod = new WitchModifier(this);
            mod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private void ApplyRandomDebuff()
        {
            BattleController.AddModMessage(BattleActor.ENEMY, "Hex!");
            float randomValue = Random.value;
            if (randomValue < 0.25f) // roll debuff
            {
                Modifier debuff = new RollBuffModifier(-rollDebuff, -rollDebuff);
                debuff.actor = BattleActor.PLAYER;
                debuff.isRollBounded = true;
                debuff.numRollsRemaining = debuffDuration;
                debuff.battleEffect = RollBoundedBattleEffect.DEBUFF;
                BattleController.AddStatusMessage(BattleActor.PLAYER, 
                    "-" + rollDebuff + " Roll: " + debuffDuration + " rolls");
                PlayerStatus.Status.NextRollMods.Add(debuff);
            }
            else if (randomValue < 0.5f) // break
            {
                InflictStatusOnHitModifier.AddStatusModifier(BattleActor.PLAYER,
                    StatusEffect.BREAK, debuffDuration);
            }
            else if (randomValue < 0.75f) // enraged
            {
                InflictStatusOnHitModifier.AddStatusModifier(BattleActor.PLAYER,
                    StatusEffect.ENRAGED, debuffDuration);
            }
            else // poison
            {
                InflictStatusOnHitModifier.AddStatusModifier(BattleActor.PLAYER,
                    StatusEffect.POISON, debuffDuration);
            }
        }

        private class WitchModifier : Modifier, IPostDamageModifier
        {
            private WitchBattleController controller;

            public WitchModifier(WitchBattleController controller)
            {
                this.controller = controller;
            }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                if (rollResult.CurrentRoll % controller.debuffDuration == 0)
                {
                    controller.ApplyRandomDebuff();
                }
            }
        }
    }
}