using Battle;

namespace Modifiers.Ability
{
    // Chance to survive at 0 health
    public class StalwartModifier : Modifier, IRollResultModifier
    {
        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (-initial.GetTotalHealthChange(actor) >= Status().Health && RollTrigger())
            {
                int damageReduction = 
                    (-initial.GetTotalHealthChange(actor)) - Status().Health + 1;
                initial.SetDamage(actor, initial.GetDamage(actor) - damageReduction);
                BattleController.AddModMessage(actor, "Stalwart!");
            }
            return initial;
        }
    }
}
