using System;
using UnityEngine;

public class PlayerRollGenerator : RollGenerator
{
    private Stance currentStance;

    private void Start()
    {
        currentStance = PlayerStatus.SelectedStance;
    }

    public void SetStance(Stance stance)
    {
        this.currentStance = stance;
        PlayerStatus.SelectedStance = stance;
    }

    public override int generateInitialRoll()
    {
        int min = getMinRoll();
        int max = getMaxRoll();
        foreach (RollGenerationModifier mod in PlayerStatus.Mods.GetRollGenerationModifiers())
        {
            Tuple<int, int> modified = mod.apply(min, max, PlayerStatus.SelectedStance);
            min = modified.Item1;
            max = modified.Item2;
            BattleController.DecrementAndDeregisterIfNecessary(mod);
        }
        return generateBasicRoll(min, max);
    }

    private int getMinRoll()
    {
        switch (currentStance)
        {
            case Stance.NEUTRAL:
                return minRoll;
            case Stance.DEFENSIVE:
                return minRoll + 1;
            case Stance.AGGRESSIVE:
                return minRoll - 1;
        }
        return minRoll;
    }

    private int getMaxRoll()
    {
        switch (currentStance)
        {
            case Stance.NEUTRAL:
                return maxRoll;
            case Stance.DEFENSIVE:
                return maxRoll - 1;
            case Stance.AGGRESSIVE:
                return maxRoll + 1;
        }
        return maxRoll;
    }
}
