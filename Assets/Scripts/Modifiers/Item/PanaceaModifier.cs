using System.Linq;

namespace Modifiers.Item
{
    // Deregister all active negative roll bounded mods
    public class PanaceaModifier : Modifier, IOneTimeEffectModifier
    {
        // Deregister active and remove upcoming debuffs
        public void ApplyOneTimeEffectMod()
        {
            foreach (Modifier nextRollMod in Status().GetNextRollRollBoundedBattleEffectModifiers().ToList())
            {
                if (nextRollMod.battleEffect.IsNegativeEffect())
                {
                    Status().NextRollMods.Remove(nextRollMod);
                }
            }
            foreach (Modifier activeMod in Status().GetActiveRollBoundedBattleEffectModifiers().ToList())
            {
                if (activeMod.battleEffect.IsNegativeEffect())
                {
                    activeMod.DeregisterSelf();
                }
            }
        }

        // Can only apply if there are active debuffs
        public bool CanApply()
        {
            return Status().GetNextRollRollBoundedBattleEffectModifiers().Any(m => m.battleEffect.IsNegativeEffect())
                || Status().GetActiveRollBoundedBattleEffectModifiers().Any(m => m.battleEffect.IsNegativeEffect());
        }
    }
}
