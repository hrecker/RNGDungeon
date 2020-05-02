namespace Modifiers.Tech
{
    // Reduces cooldown of the next tech used this battle to 1
    public class WarmupModifier : Modifier, ITechModifier
    {
        public WarmupModifier()
        {
            isFullBattleModifier = true;
        }

        public int ApplyTechCooldownModifier(Data.Tech tech, bool isSelectedTech, 
            int startingCooldown)
        {
            if (isFullBattleModifier && isSelectedTech && 
                tech != Data.Cache.GetTech("Warm-up"))
            {
                startingCooldown = 1;
                isFullBattleModifier = false;
            }
            return startingCooldown;
        }
    }
}
