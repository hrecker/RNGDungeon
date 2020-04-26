using UnityEngine;
using Modifiers;
using System;

namespace Battle
{
    public class RollGenerator : MonoBehaviour
    {
        public BattleActor actor;

        public int GenerateInitialRoll(Data.Tech tech)
        {
            int min = actor.Status().BaseMinRoll;
            int max = actor.Status().BaseMaxRoll;
            foreach (IRollGenerationModifier mod in actor.Status().Mods.GetRollGenerationModifiers())
            {
                Tuple<int, int> modified = mod.ApplyRollGenerationMod(tech, min, max);
                min = modified.Item1;
                max = modified.Item2;
            }
            if (min > max)
            {
                min = max;
            }
            return GenerateBasicRoll(min, max);
        }

        private int GenerateBasicRoll(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}
