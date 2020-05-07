using Battle;
using Modifiers.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modifiers.StatusEffect
{
    // Buffs against opponents inflicted with a status
    public class StatusPunishingRollBuffModifier : RollBuffModifier
    {
        private int punishMinRollBuff;
        private int punishMaxRollBuff;
        private string modMessage;
        private List<Battle.StatusEffect> punishedEffects;

        public StatusPunishingRollBuffModifier(
            int punishMinRollBuff, int punishMaxRollBuff) : 
            this(punishMinRollBuff, punishMaxRollBuff, null, null) { }

        public StatusPunishingRollBuffModifier(
            int punishMinRollBuff, int punishMaxRollBuff,
            string modMessage) : 
            this(punishMinRollBuff, punishMaxRollBuff, modMessage, null) { }

        public StatusPunishingRollBuffModifier(
            int punishMinRollBuff, int punishMaxRollBuff,
            string modMessage,
            List<Battle.StatusEffect> punishedEffects) : base(0, 0)
        {
            this.punishMinRollBuff = punishMinRollBuff;
            this.punishMaxRollBuff = punishMaxRollBuff;
            this.modMessage = modMessage;
            if (punishedEffects != null)
            {
                this.punishedEffects = punishedEffects;
            }
            else
            {
                this.punishedEffects = new List<Battle.StatusEffect>()
                { 
                    Battle.StatusEffect.BREAK,
                    Battle.StatusEffect.ENRAGED,
                    Battle.StatusEffect.POISON
                };
            }
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
                    if (punishedEffects.Contains(mod.statusEffect))
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
