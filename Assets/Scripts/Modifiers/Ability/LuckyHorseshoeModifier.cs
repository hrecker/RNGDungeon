namespace Modifiers.Ability
{
    public class LuckyHorseshoeModifier : Modifier, IOneTimeEffectModifier
    {
        private int luckBuff = 3;

        public void ApplyOneTimeEffectMod()
        {
            PlayerStatus.Luck += luckBuff;
        }

        public bool CanApply()
        {
            return true;
        }
    }
}
