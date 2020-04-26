namespace Modifiers.Ability
{
    // Mod that reduces tech cooldowns by 2
    public class HighStaminaModifier : Modifier, ITechModifier
    {
        public int ApplyTechCooldownModifier(Data.Tech tech, int startingCooldown)
        {
            return startingCooldown - 2;
        }
    }
}
