using Modifiers;
using Modifiers.Generic;

namespace Battle.Enemies
{
    // Buffs on consecutive hits to the player, up to +5 roll
    public class WolfPackBattleController : EnemyBattleController
    {
        public void Start()
        {
            Modifier mod = new WolfPackModifier();
            mod.actor = BattleActor.ENEMY;
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private class WolfPackModifier : RollBuffModifier, IPostDamageModifier
        {
            private int buffPerConsecutiveHit = 1;
            private int resetBuff = 6;

            public WolfPackModifier() : base(0, 0) { }

            public void ApplyPostDamageMod(RollResult rollResult)
            {
                if (rollResult.GetRollDamage(BattleActor.PLAYER) > 0)
                {
                    battleEffect = RollBoundedBattleEffect.BUFF;
                    if (minRollDiff < resetBuff)
                    {
                        BattleController.AddModMessage(BattleActor.ENEMY, "Momentum!");
                        BattleController.AddStatusMessage(BattleActor.ENEMY, 
                            "+" + buffPerConsecutiveHit + " roll");
                        minRollDiff += buffPerConsecutiveHit;
                        maxRollDiff += buffPerConsecutiveHit;
                    }
                    else
                    {
                        // Reset buff after a certain number of rolls so the player
                        // can't get completely locked out
                        battleEffect = RollBoundedBattleEffect.NONE;
                        minRollDiff = 0;
                        maxRollDiff = 0;
                    }
                }
                else
                {
                    battleEffect = RollBoundedBattleEffect.NONE;
                    minRollDiff = 0;
                    maxRollDiff = 0;
                }
            }
        }
    }
}