namespace Modifiers.Ability
{
    // Increases luck
    public class LuckyHorseshoeModifier : Modifier, IOneTimeEffectModifier
    {
        private int luckBuff = 3;

        public void ApplyOneTimeEffectMod()
        {
            Status().Luck += luckBuff;
        }

        public bool CanApply()
        {
            return true;
        }
    }
}
