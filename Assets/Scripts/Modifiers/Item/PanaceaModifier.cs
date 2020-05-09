using System.Linq;

namespace Modifiers.Item
{
    // Deregister all active negative roll bounded mods
    public class PanaceaModifier : Modifier, IOneTimeEffectModifier
    {
        // Deregister active and remove upcoming debuffs
        public virtual void ApplyOneTimeEffectMod()
        {
            foreach (Modifier nextRollMod in Status().GetNextRollRollBoundedBattleEffectModifiers().ToList())
            {
                if (nextRollMod.GetBattleEffect().IsNegativeEffect())
                {
                    Status().NextRollMods.Remove(nextRollMod);
                }
            }
            foreach (Modifier activeMod in Status().GetActiveRollBoundedBattleEffectModifiers().ToList())
            {
                if (activeMod.GetBattleEffect().IsNegativeEffect())
                {
                    activeMod.DeregisterSelf();
                }
            }
        }

        // Can only apply if there are active debuffs
        public virtual bool CanApply()
        {
            return Status().GetNextRollRollBoundedBattleEffectModifiers().Any(m => m.GetBattleEffect().IsNegativeEffect())
                || Status().GetActiveRollBoundedBattleEffectModifiers().Any(m => m.GetBattleEffect().IsNegativeEffect());
        }
    }
}
