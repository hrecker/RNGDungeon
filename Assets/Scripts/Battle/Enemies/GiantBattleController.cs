using Modifiers.Generic;
using Modifiers.StatusEffect;
using Modifiers;

namespace Battle.Enemies
{
    // Alternates between self buff/debuff every two turns, and can cause break
    public class GiantBattleController : EnemyBattleController
    {
        public void Start()
        {
            Modifier rollMod = new GiantModifier();
            rollMod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(rollMod);
            Modifier breakMod = new InflictStatusOnHitModifier(StatusEffect.BREAK, 3);
            breakMod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(breakMod);
        }

        private class GiantModifier : RollBuffModifier, IPostDamageModifier
        {
            private int effectDuration = 3;
            private int rollDiff = 2;
            private bool currentlyBuffed;

            public GiantModifier() : base(0, 0) { }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                if (rollResult.CurrentRoll % effectDuration == 0)
                {
                    if (currentlyBuffed)
                    {
                        currentlyBuffed = false;
                        minRollDiff = -rollDiff;
                        maxRollDiff = 0;
                        battleEffect = RollBoundedBattleEffect.DEBUFF;
                        BattleController.AddModMessage(BattleActor.ENEMY, "Moody!");
                        BattleController.AddStatusMessage(BattleActor.ENEMY,
                            "-" + rollDiff + " min roll");
                    }
                    else
                    {
                        currentlyBuffed = true;
                        minRollDiff = 0;
                        maxRollDiff = rollDiff;
                        battleEffect = RollBoundedBattleEffect.BUFF;
                        BattleController.AddModMessage(BattleActor.ENEMY, "Moody!");
                        BattleController.AddStatusMessage(BattleActor.ENEMY,
                            "+" + rollDiff + " max roll");
                    }
                }
            }
        }
    }
}