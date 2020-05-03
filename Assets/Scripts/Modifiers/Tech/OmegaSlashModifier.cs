using Battle;
using Modifiers.Generic;

namespace Modifiers.Tech
{
    // Provides huge buff, but puts all techs on cooldown
    public class OmegaSlashModifier : RollBuffModifier
    {
        public OmegaSlashModifier(int minRollBuff, int maxRollBuff) : base(minRollBuff, maxRollBuff) { }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            BattleController.AddModMessage(actor, "OmegaSlash!");
            // Put all techs on cooldown
            foreach (Data.Tech tech in PlayerStatus.EnabledTechs)
            {
                if (tech != currentRollGen.PlayerTech)
                {
                    tech.scheduledCooldownActivate = true;
                }
            }
            return base.ApplyRollGenerationMod(currentRollGen);
        }
    }
}
