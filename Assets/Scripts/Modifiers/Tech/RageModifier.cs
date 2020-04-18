using System;
using Battle;

namespace Modifiers.Tech
{
    // Deal more damage when below 50% health
    public class RageModifier : Modifier, IRollGenerationModifier
    {
        // Activates when less than half health
        private const float healthThreshold = 0.5f;
        // The buff is based on a fraction of the current max roll. 
        private const float rollBuffFraction = 0.34f;

        public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
        {
            if ((float)Status().Health / Status().MaxHealth < healthThreshold)
            {
                BattleController.AddModMessage(actor, "Rage!");
                int buff = (int)Math.Max(1, rollBuffFraction * initialMaxRoll);
                initialMinRoll += buff;
                initialMaxRoll += buff;
            }
            return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
        }
    }
}
