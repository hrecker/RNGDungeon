using System;
using Modifiers;

namespace Battle
{
    public class PlayerRollGenerator : RollGenerator
    {
        private void Start()
        {
            minRoll = PlayerStatus.BaseMinRoll;
            maxRoll = PlayerStatus.BaseMaxRoll;
        }

        public override int GenerateInitialRoll()
        {
            int min = minRoll;
            int max = maxRoll;
            foreach (IRollGenerationModifier mod in PlayerStatus.Mods.GetRollGenerationModifiers())
            {
                Tuple<int, int> modified = mod.ApplyRollGenerationMod(min, max);
                min = modified.Item1;
                max = modified.Item2;
            }
            if (min > max)
            {
                min = max;
            }
            return GenerateBasicRoll(min, max);
        }
    }
}
