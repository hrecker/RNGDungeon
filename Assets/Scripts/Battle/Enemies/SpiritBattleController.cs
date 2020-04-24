using Modifiers;
using Modifiers.Generic;

namespace Battle.Enemies
{
    // Enemy that curses player with 3 turn debuff when taking damage
    public class SpiritBattleController : EnemyBattleController
    {
        private int rollDebuff = 1;
        private int rollDebuffTurns = 3;

        private void Start()
        {
            SpritModifier mod = new SpritModifier(this);
            EnemyStatus.Status.Mods.RegisterModifier(mod);
        }

        private class SpritModifier : Modifier, IPostDamageModifier
        {
            SpiritBattleController controller;

            public SpritModifier(SpiritBattleController controller)
            {
                this.controller = controller;
                actor = BattleActor.ENEMY;
            }

            public void ApplyPostDamageMod(RollResult initial)
            {
                // Apply curse when hit by roll damage
                if (initial.EnemyRollDamage > 0)
                {
                    Modifier mod = new RollBuffModifier(-controller.rollDebuff, 
                        -controller.rollDebuff);
                    mod.isRollBounded = true;
                    mod.numRollsRemaining = controller.rollDebuffTurns;
                    mod.battleEffect = RollBoundedBattleEffect.DEBUFF;
                    BattleController.AddStatusMessage(BattleActor.PLAYER, "-1 Roll: 3 turns");
                    PlayerStatus.Status.NextRollMods.Add(mod);
                    BattleController.AddModMessage(BattleActor.ENEMY, "Curse!");
                }
            }
        }
    }
}
