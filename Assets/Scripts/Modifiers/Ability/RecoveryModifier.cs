using Battle;

namespace Modifiers.Ability
{
    // Recover some health after each battle won
    public class RecoveryModifier : Modifier, IPostBattleModifier
    {
        private int healthRecovered;

        public RecoveryModifier(int healthRecovered)
        {
            this.healthRecovered = healthRecovered;
        }

        public void ApplyPostBattleMod()
        {
            if (Status().Health < Status().MaxHealth)
            {
                Status().Health += healthRecovered;
                BattleController.AddModMessage(actor, "Recovery!");
            }
        }
    }
}
