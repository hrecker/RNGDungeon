using UnityEngine;
using Battle;

namespace Modifiers
{
    // base class of all modifiers
    public abstract class Modifier
    {
        // Who this modifier applies to (player or enemy)
        public BattleActor actor = BattleActor.PLAYER;
        // Priority when applying all modifiers - lower numbers occur first, higher numbers later
        // Most modifiers should have priority 0. RollResult mods that reduce damage or have effects based
        // on damage done may need higher (later) priorities
        public int priority;
        // Whether this modifier only exists for a certain number of rolls in battle
        public bool isRollBounded;
        // Number of rolls that this modifier should exist for (if applicable)
        public int numRollsRemaining;
        // 0-1 chance that this modifier triggers (if applicable)
        public float triggerChance;

        // Randomly determine if this modifier should be triggered
        protected bool RollTrigger()
        {
            float chance = Status().GetTriggerChanceWithLuck(triggerChance);
            if (chance <= 0)
            {
                return false;
            }
            if (chance >= 1)
            {
                return true;
            }
            return Random.value < chance;
        }

        public void DeregisterSelf()
        {
            Status().Mods.DeregisterModifier(this);
        }

        protected BattleStatus Status()
        {
            return actor.Status();
        }
    }
}
