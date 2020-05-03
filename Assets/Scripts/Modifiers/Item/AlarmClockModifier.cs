using Battle;
using Modifiers.Generic;

namespace Modifiers.Item
{
    // Creates a buff every x rolls
    public class AlarmClockModifier : Modifier, IRollGenerationModifier
    {
        private int rollsBetweenBuffs;
        private int buffDuration;
        private int minRollDiff;
        private int maxRollDiff;

        public AlarmClockModifier(int minRollDiff, int maxRollDiff,
            int rollsBetweenBuffs, int buffDuration)
        {
            this.minRollDiff = minRollDiff;
            this.maxRollDiff = maxRollDiff;
            this.rollsBetweenBuffs = rollsBetweenBuffs;
            this.buffDuration = buffDuration;
        }

        public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            if (currentRollGen.CurrentRoll % rollsBetweenBuffs == 0)
            {
                Modifier mod = new RollBuffModifier(minRollDiff, maxRollDiff);
                mod.actor = actor;
                mod.isRollBounded = true;
                mod.numRollsRemaining = buffDuration;
                mod.battleEffect = RollBoundedBattleEffect.BUFF;
                BattleController.AddStatusMessage(actor, "+" + minRollDiff + 
                    " Roll: " + buffDuration + " rolls");
                actor.Status().NextRollMods.Add(mod);
                BattleController.AddModMessage(actor, "Alarm Clock!");
            }
            return currentRollGen;
        }
    }
}
