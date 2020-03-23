using System;
using UnityEngine;

public class PlayerRollGenerator : RollGenerator
{
    //TODO stances
    private string currentStance;

    private void Start()
    {
        currentStance = PlayerStatus.SelectedStance;
    }

    public void SetStance(string stanceName)
    {
        this.currentStance = stanceName;
        PlayerStatus.SelectedStance = stanceName;
    }

    public override Tuple<int, int> applyPostRollModifiers(Tuple<int, int> playerEnemyRolls)
    {
        //TODO
        return playerEnemyRolls;
    }

    public override int generateInitialRoll()
    {
        return generateBasicRoll(getMinRoll(), getMaxRoll());
    }

    private int getMinRoll()
    {
        //TODO probably will want stance numbers to be in some configurable location outside of code
        if (currentStance == null || currentStance == "Neutral")
        {
            return minRoll;
        }
        else if (currentStance == "Defensive")
        {
            return minRoll + 1;
        }
        else if (currentStance == "Aggressive")
        {
            return minRoll - 1;
        }
        return minRoll;
    }

    private int getMaxRoll()
    {
        //TODO probably will want stance numbers to be in some configurable location outside of code
        if (currentStance == null || currentStance == "Neutral")
        {
            return maxRoll;
        }
        else if (currentStance == "Defensive")
        {
            return maxRoll - 1;
        }
        else if (currentStance == "Aggressive")
        {
            return maxRoll + 1;
        }
        return maxRoll;
    }
}
