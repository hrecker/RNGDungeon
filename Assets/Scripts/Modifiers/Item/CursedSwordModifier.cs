using Battle;
using Modifiers.Generic;

namespace Modifiers.Item
{
    // Randomly buffs or debuffs every 3 rolls, based on luck
    public class CursedSwordModifier : Modifier, IPostDamageModifier, IPostBattleModifier
    {
        private int rollDiff;
        private int rollsPassed;

        public CursedSwordModifier(int rollDiff)
        {
            this.rollDiff = rollDiff;
        }

        public void ApplyPostBattleMod()
        {
            rollsPassed = 0;
        }

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            rollsPassed++;
            if (rollsPassed % 3 == 0)
            {
                Modifier rollBuffMod;
                string statusMessage;
                if (RollTrigger())
                {
                    rollBuffMod = new RollBuffModifier(rollDiff, rollDiff);
                    rollBuffMod.SetBattleEffect(RollBoundedBattleEffect.BUFF);
                    statusMessage = "+";
                }
                else
                {
                    rollBuffMod = new RollBuffModifier(-rollDiff, -rollDiff);
                    rollBuffMod.SetBattleEffect(RollBoundedBattleEffect.DEBUFF);
                    statusMessage = "-";
                }
                statusMessage += rollDiff + " Roll: 3 rolls";
                BattleController.AddStatusMessage(BattleActor.PLAYER, statusMessage);
                rollBuffMod.actor = BattleActor.PLAYER;
                rollBuffMod.isRollBounded = true;
                rollBuffMod.numRollsRemaining = 3;
                PlayerStatus.Status.NextRollMods.Add(rollBuffMod);
                rollsPassed = 0;
            }
        }
    }
}
