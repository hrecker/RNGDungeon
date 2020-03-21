using System.Collections.Generic;
using UnityEngine;

public class RollResultGenerator : MonoBehaviour
{
    //TODO list of RollResultModifiers here to apply
    List<RollResultModifier> modifiers;

    public RollResult applyModifiers(RollResult currentResult)
    {
        if (modifiers != null)
        {
            foreach (RollResultModifier mod in modifiers)
            {
                currentResult = mod.apply(currentResult);
            }
        }
        return currentResult;
    }

    //TODO may add a post-damage result for stuff like health leeching?
}
