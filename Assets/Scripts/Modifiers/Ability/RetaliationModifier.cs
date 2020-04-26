using Battle;

namespace Modifiers.Ability
{
    // Decreases any active tech cooldowns when taking roll damage
    public class RetaliationModifier : Modifier, IPostDamageModifier
    {
        public void ApplyPostDamageMod(RollResult initial)
        {
            if (initial.GetRollDamage(actor) > 0)
            {
                bool anyActiveTechs = false;
                foreach (Data.Tech tech in PlayerStatus.EnabledTechs)
                {
                    anyActiveTechs |= tech.GetCurrentCooldown() > 1;
                    tech.DecrementCooldown();
                }
                if (anyActiveTechs)
                {
                    BattleController.AddModMessage(actor, "Retaliation!");
                }
            }
        }
    }
}
