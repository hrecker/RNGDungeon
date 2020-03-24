using System.Collections.Generic;
using UnityEngine;

public class RollResultGenerator : MonoBehaviour
{
    // List of rollresultmodifiers to apply, mapped to the
    // remaining number of rolls they should apply for
    // before being removed.
    Dictionary<RollResultModifier, int> modifiers;

    private void Start()
    {
        modifiers = new Dictionary<RollResultModifier, int>();
    }

    // If the modifier should remain active perpetually, rollCount can be set to -1
    public void AddModifier(RollResultModifier mod, int rollCount)
    {
        modifiers.Add(mod, rollCount);
    }

    public RollResult applyModifiers(RollResult currentResult)
    {
        if (modifiers != null)
        {
            List<RollResultModifier> toRemove = new List<RollResultModifier>();
            foreach (RollResultModifier mod in new List<RollResultModifier>(modifiers.Keys))
            {
                currentResult = mod.apply(currentResult);
                modifiers[mod]--;
                if (modifiers[mod] == 0)
                {
                    toRemove.Add(mod);
                }
            }
            foreach (RollResultModifier mod in toRemove)
            {
                modifiers.Remove(mod);
            }
        }
        return currentResult;
    }

    //TODO may add a post-damage result for stuff like health leeching?
}
