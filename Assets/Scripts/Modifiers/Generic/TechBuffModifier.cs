using Battle;

namespace Modifiers.Generic
{
    // Buffs roll for all techs
    public class TechBuffModifier : RollBuffModifier
    {
        private int techMinRollBuff;
        private int techMaxRollBuff;

        public TechBuffModifier(int techMinRollBuff, int techMaxRollBuff) : base(0, 0)
        {
            this.techMinRollBuff = techMinRollBuff;
            this.techMaxRollBuff = techMaxRollBuff;
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            if (currentRollGen.PlayerTech != null)
            {
                minRollDiff = techMinRollBuff;
                maxRollDiff = techMaxRollBuff;
            }
            currentRollGen = base.ApplyRollGenerationMod(currentRollGen);
            minRollDiff = 0;
            maxRollDiff = 0;
            return currentRollGen;
        }
    }
}
