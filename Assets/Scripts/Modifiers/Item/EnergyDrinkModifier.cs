using System.Linq;

namespace Modifiers.Item
{
    // Reduces all active tech cooldowns by 1
    public class EnergyDrinkModifier : Modifier, IOneTimeEffectModifier
    {
        public void ApplyOneTimeEffectMod()
        {
            foreach (Data.Tech tech in PlayerStatus.EnabledTechs)
            {
                tech.DecrementCooldown();
            }
        }

        public bool CanApply()
        {
            return PlayerStatus.EnabledTechs.Any(t => t.GetCurrentCooldown() > 0);
        }
    }
}
