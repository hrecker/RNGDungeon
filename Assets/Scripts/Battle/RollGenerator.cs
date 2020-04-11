using UnityEngine;

namespace Battle
{
    public abstract class RollGenerator : MonoBehaviour
    {
        protected int minRoll;
        protected int maxRoll;

        // Generate an initial roll value
        public abstract int GenerateInitialRoll();

        protected int GenerateBasicRoll(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}
