using UnityEngine;
using Modifiers;
using System;

namespace Battle
{
    public class RollGenerator : MonoBehaviour
    {
        public BattleActor actor;

        public int GenerateInitialRoll(Data.Tech tech, int currentRoll)
        {
            RollGeneration rollGen = new RollGeneration()
            {
                MinRoll = actor.Status().BaseMinRoll,
                MaxRoll = actor.Status().BaseMaxRoll,
                PlayerTech = tech,
                CurrentRoll = currentRoll
            };
            foreach (IRollGenerationModifier mod in actor.Status().Mods.GetRollGenerationModifiers())
            {
                rollGen = mod.ApplyRollGenerationMod(rollGen);
            }
            if (rollGen.MinRoll > rollGen.MaxRoll)
            {
                rollGen.MinRoll = rollGen.MaxRoll;
            }
            return GenerateBasicRoll(rollGen.MinRoll, rollGen.MaxRoll);
        }

        private int GenerateBasicRoll(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}
