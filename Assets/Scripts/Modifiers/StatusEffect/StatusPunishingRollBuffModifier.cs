using Battle;
using Modifiers.Generic;

namespace Modifiers.StatusEffect
{
    // Buffs against opponents inflicted with a status
    public class StatusPunishingRollBuffModifier : RollBuffModifier
    {
        private int punishMinRollBuff;
        private int punishMaxRollBuff;
        private string modMessage;

        public StatusPunishingRollBuffModifier(
            int punishMinRollBuff, int punishMaxRollBuff) : 
            this(punishMinRollBuff, punishMaxRollBuff, null) { }

        public StatusPunishingRollBuffModifier(
            int punishMinRollBuff, int punishMaxRollBuff,
            string modMessage) : base(0, 0)
        {
            this.punishMinRollBuff = punishMinRollBuff;
            this.punishMaxRollBuff = punishMaxRollBuff;
            this.modMessage = modMessage;
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            if (BattleController.isInBattle)
            {
                if (modMessage != null)
                {
                    BattleController.AddModMessage(actor, modMessage);
                }
                foreach (Modifier mod in actor.Opponent().Status().
                    GetActiveRollBoundedBattleEffectModifiers())
                {
                    if (mod.statusEffect != Battle.StatusEffect.NONE)
                    {
                        minRollDiff = punishMinRollBuff;
                        maxRollDiff = punishMaxRollBuff;
                        break;
                    }
                }
            }
            currentRollGen = base.ApplyRollGenerationMod(currentRollGen);
            minRollDiff = 0;
            maxRollDiff = 0;
            return currentRollGen;
        }
    }
}
