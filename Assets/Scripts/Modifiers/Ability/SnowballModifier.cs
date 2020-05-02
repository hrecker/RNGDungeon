using Battle;

namespace Modifiers.Ability
{
    // Ability that reduces cooldown for techs that hit successfully
    public class SnowballModifier : Modifier, IPostDamageModifier, ITechModifier
    {
        private int cooldownReduction = 2;
        private Data.Tech activeTech;
        private bool damageDealt;

        public void ApplyPostDamageMod(RollResult rollResult)
        {
            activeTech = rollResult.PlayerTech;
            damageDealt = rollResult.EnemyRollDamage > 0;
        }

        public int ApplyTechCooldownModifier(Data.Tech tech, bool isSelectedTech,
            int startingCooldown)
        {
            if (damageDealt && activeTech == tech)
            {
                startingCooldown -= cooldownReduction;
                BattleController.AddModMessage(actor, "Snowball!");
            }
            return startingCooldown;
        }
    }
}
