using Battle;

namespace Modifiers.Generic
{
    // Changes luck
    public class LuckBuffModifier : Modifier, IOneTimeEffectModifier
    {
        private int luckBuff;
        private string modMessage;
        private bool addRollStatusMessage;

        public LuckBuffModifier(int luckBuff) : this(luckBuff, null, true) { }

        public LuckBuffModifier(int luckBuff, string modMessage) : 
            this(luckBuff, modMessage, true) { }

        public LuckBuffModifier(int luckBuff, string modMessage, bool addRollStatusMessage)
        {
            this.luckBuff = luckBuff;
            this.modMessage = modMessage;
            this.addRollStatusMessage = addRollStatusMessage;
        }

        public void ApplyOneTimeEffectMod()
        {
            Status().Luck += luckBuff;
            if (modMessage != null)
            {
                BattleController.AddModMessage(actor, modMessage);
            }
            if (addRollStatusMessage)
            {
                BattleController.AddStatusMessage(actor, "+" + luckBuff + 
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
