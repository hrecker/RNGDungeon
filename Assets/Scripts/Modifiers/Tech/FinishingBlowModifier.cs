using Battle;
using Modifiers.Generic;

namespace Modifiers.Tech
{
    // Buffs against opponents inflicted with a status
    public class FinishingBlow : RollBuffModifier
    {
        private int fullMinRollBuff;
        private int fullMaxRollBuff;

        public FinishingBlow(int fullMinRollBuff, int fullMaxRollBuff) : base(0, 0)
        {
            this.fullMinRollBuff = fullMinRollBuff;
            this.fullMaxRollBuff = fullMaxRollBuff;
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            BattleController.AddModMessage(actor, "Finishing Blow!");
            foreach (Modifier mod in actor.Opponent().Status().
                GetActiveRollBoundedBattleEffectModifiers())
            {
                if (mod.statusEffect != Battle.StatusEffect.NONE)
                {
                    minRollDiff = fullMinRollBuff;
                    maxRollDiff = fullMaxRollBuff;
                    break;
                }
            }
            currentRollGen = base.ApplyRollGenerationMod(currentRollGen);
            minRollDiff = 0;
            maxRollDiff = 0;
            return currentRollGen;
        }
    }
}
