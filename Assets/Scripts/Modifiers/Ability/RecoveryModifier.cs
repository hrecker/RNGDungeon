using Battle;

namespace Modifiers.Ability
{
    public class RecoveryModifier : Modifier, IPostBattleModifier
    {
        private int healthRecovered;

        public RecoveryModifier(int healthRecovered)
        {
            this.healthRecovered = healthRecovered;
        }

        public void ApplyPostBattleMod()
        {
            if (PlayerStatus.Health < PlayerStatus.MaxHealth)
            {
                PlayerStatus.Health += healthRecovered;
                BattleController.AddPlayerModMessage("Recovery!");
            }
        }
    }
}
