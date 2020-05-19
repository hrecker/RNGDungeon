namespace Modifiers.Tech
{
    // Reduces cooldown of the next tech used this battle to 1
    public class WarmupModifier : Modifier, IOneTimeEffectModifier
    {
        public void ApplyOneTimeEffectMod()
        {
            Modifier innerMod = new FullBattleWarmupModifier();
            innerMod.actor = actor;
            innerMod.isRollBounded = false;
            actor.Status().Mods.RegisterModifier(innerMod);
        }

        public bool CanApply()
        {
            return true;
        }

        // Create a non-rollbounded modifier to do the work of this tech - otherwise this
        // modifier will get deregistered before having its intended effect if the tech
        // used after WarmUp is the very last tech of the battle
        private class FullBattleWarmupModifier : Modifier, ITechModifier, IPostBattleModifier
        {
            public void ApplyPostBattleMod()
            {
                DeregisterSelf();
            }

            public int ApplyTechCooldownModifier(Data.Tech tech, bool isSelectedTech, int startingCooldown)
            {
                if (isSelectedTech && tech != Data.Cache.GetTech("Warm-up"))
                {
                    startingCooldown = 1;
                    DeregisterSelf();
                }
                return startingCooldown;
            }
        }

    }
}
