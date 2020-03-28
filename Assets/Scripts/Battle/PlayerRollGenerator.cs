using System;

public class PlayerRollGenerator : RollGenerator
{
    public override int generateInitialRoll()
    {
        int min = minRoll;
        int max = maxRoll;
        foreach (IRollGenerationModifier mod in PlayerStatus.Mods.GetRollGenerationModifiers())
        {
            Tuple<int, int> modified = mod.apply(min, max);
            min = modified.Item1;
            max = modified.Item2;
            BattleController.DecrementAndDeregisterIfNecessary((Modifier)mod);
        }
        return generateBasicRoll(min, max);
    }
}
