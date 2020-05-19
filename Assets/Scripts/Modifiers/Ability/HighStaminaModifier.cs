namespace Modifiers.Ability
{
    // Mod that reduces tech cooldowns by 1
    public class HighStaminaModifier : Modifier, ITechModifier
    {
        public int ApplyTechCooldownModifier(Data.Tech tech, bool isSelectedTech,
            int startingCooldown)
        {
            return startingCooldown - 1;
        }
    }
}
