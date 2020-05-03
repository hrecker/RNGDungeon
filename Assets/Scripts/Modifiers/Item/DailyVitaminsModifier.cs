namespace Modifiers.Item
{
    // Modifies the cooldown of the first tech used each battle
    public class DailyVitaminsModifier : Modifier, ITechModifier, IPostBattleModifier
    {
        private int cooldownDiff;
        private bool reductionApplied;

        public DailyVitaminsModifier(int cooldownDiff)
        {
            this.cooldownDiff = cooldownDiff;
        }

        public void ApplyPostBattleMod()
        {
            // Reset for next battle
            reductionApplied = false;
        }

        public int ApplyTechCooldownModifier(Data.Tech tech, bool isSelectedTech, int startingCooldown)
        {
            if (!reductionApplied && isSelectedTech)
            {
                startingCooldown += cooldownDiff;
                reductionApplied = true;
            }
            return startingCooldown;
        }
    }
}
