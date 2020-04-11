using System;

public class RageModifier : Modifier, IRollGenerationModifier
{
    // Activates when less than half health
    private const float playerHealthThreshold = 0.5f; 
    // The buff is based on a fraction of the current max roll. 
    private const float rollBuffFraction = 0.34f;

    public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
    {
        BattleController.AddPlayerModMessage("Rage!");
        if ((float) PlayerStatus.Health / PlayerStatus.MaxHealth < playerHealthThreshold)
        {
            int buff = (int) Math.Max(1, rollBuffFraction * initialMaxRoll);
            initialMinRoll += buff;
            initialMaxRoll += buff;
        }
        return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
    }
}
