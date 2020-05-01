using Battle;

namespace Modifiers.Generic
{
    // Changes luck
    public class LuckBuffModifier : Modifier, IOneTimeEffectModifier
    {
        private int luckBuff;
        private string modMessage;

        public LuckBuffModifier(int luckBuff) : this(luckBuff, null) { }

        public LuckBuffModifier(int luckBuff, string modMessage)
        {
            this.luckBuff = luckBuff;
            this.modMessage = modMessage;
        }

        public void ApplyOneTimeEffectMod()
        {
            Status().Luck += luckBuff;
            if (modMessage != null)
            {
                BattleController.AddModMessage(actor, modMessage);
            }
            if (isRollBounded)
            {
                BattleController.AddStatusMessage(actor, "+ " + luckBuff + 
                    " Luck: " + numRollsRemaining + " rolls");
            }
        }

        public bool CanApply()
        {
            return true;
        }

        protected override void OnDeregister()
        {
            Status().Luck -= luckBuff;
        }
    }
}
