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
                Status().NextRollMods.Remove(nextRollMod);
            }
            foreach (Modifier activeMod in Status().GetActiveRollBoundedBattleEffectModifiers().ToList())
            {
                activeMod.DeregisterSelf();
            }
        }

        // Can only apply if there are active debuffs
        public bool CanApply()
        {
            return Status().GetNextRollRollBoundedBattleEffectModifiers().GetEnumerator().MoveNext()
                || Status().GetActiveRollBoundedBattleEffectModifiers().GetEnumerator().MoveNext();
        }
    }
}
