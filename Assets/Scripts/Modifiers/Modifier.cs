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
        // Status caused by this modifier, if any
        public Battle.StatusEffect statusEffect = Battle.StatusEffect.NONE;
        // General type of temporary effect this mod has in battle.
        // Only applies to roll bounded mods in general, or enemies with phases.
        public RollBoundedBattleEffect battleEffect = RollBoundedBattleEffect.NONE;
        // Whether this mod has been deregistered
        public bool isDeregistered;

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
            isDeregistered = true;
            OnDeregister();
            Status().Mods.DeregisterModifier(this);
        }

        protected virtual void OnDeregister() { }

        protected BattleStatus Status()
        {
            return actor.Status();
        }
    }

    // General enum describing the type of effect this modifier has in battle
    // Should only be set for effects that end after some time in battle
    // (roll bounded mods, and/or enemies that have phases)
    // Used to display effect icons and determine what modifiers should
    // be removed when a cure effect occurs
    public enum RollBoundedBattleEffect
    {
        NONE,
        BUFF,
        DEBUFF,
        BREAK,
        POISON,
        BLOCK,
        RECOIL
    }

    public static class RollBoundedBattleEffectExtensions
    {
        // Determine if this effect is negative (the actor would choose to cure it
        // if possible) or not.
        public static bool IsNegativeEffect(this RollBoundedBattleEffect effect)
        {
            switch (effect)
            {
                case RollBoundedBattleEffect.NONE:
                case RollBoundedBattleEffect.BUFF:
                case RollBoundedBattleEffect.BLOCK:
                case RollBoundedBattleEffect.RECOIL:
                    return false;
                case RollBoundedBattleEffect.DEBUFF:
                case RollBoundedBattleEffect.BREAK:
                case RollBoundedBattleEffect.POISON:
                    return true;
            }
            return false;
        }
    }
}
