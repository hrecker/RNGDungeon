using Modifiers;
using Modifiers.StatusEffect;

namespace Battle.Enemies
{
    // Taunting fairies can enrage player (20%) and have a chance to use a heal (25%)
    public class TauntingFairyBattleController : EnemyBattleController
    {
        private void Start()
        {
            InflictStatusOnHitModifier inflictMod = new InflictStatusOnHitModifier(
                StatusEffect.ENRAGED, 2);
            inflictMod.actor = BattleActor.ENEMY;
            inflictMod.triggerChance = 0.2f;
            EnemyStatus.Status.Mods.RegisterModifier(inflictMod);
            TauntingFairyModifier fairyMod = new TauntingFairyModifier();
            fairyMod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(fairyMod);
        }

        private class TauntingFairyModifier : Modifier, IRollGenerationModifier, IRollResultModifier
        {
            private int regenRate = 2;
            private float healChance = 0.25f;
            private bool healing;
            private int healDebuff = 1;

            public TauntingFairyModifier()
            {
                triggerChance = healChance;
            }

            public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
            {
                if (Status().Health < Status().MaxHealth && RollTrigger())
                {
                    BattleController.AddModMessage(BattleActor.ENEMY, "Regen!");
                    currentRollGen.MinRoll -= healDebuff;
                    currentRollGen.MaxRoll -= healDebuff;
                    healing = true;
                }
                return currentRollGen;
            }

            public RollResult ApplyRollResultMod(RollResult initial)
            {
                if (healing)
                {
                    initial.AddHeal(actor, regenRate);
                }
                healing = false;
                return initial;
            }
        }
    }
}
