using Modifiers;
using System.Collections.Generic;
using Modifiers.StatusEffect;
using System.Linq;

namespace Battle
{
    public class BattleStatus
    {
        public BattleActor Actor { get; set; }

        private int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
            set
            {
                maxHealth = value;
                if (maxHealth < 0)
                {
                    maxHealth = 0;
                }
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
            }
        }

        private int health;
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health < 0)
                {
                    health = 0;
                }
                else if (health > MaxHealth)
                {
                    health = MaxHealth;
                }
            }
        }

        public int BaseMinRoll { get; set; }
        public int BaseMaxRoll { get; set; }

        public int Luck { get; set; }

        public List<StatusEffect> ActiveEffects { get; set; }

        private ActiveModifiers mods;
        public ActiveModifiers Mods { get { return mods; } }

        // Mods that should be added just before the next roll
        private List<Modifier> nextRollMods;
        public List<Modifier> NextRollMods { get { return nextRollMods; } }

        public BattleStatus(int maxHealth, int minRoll, int maxRoll)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            BaseMaxRoll = maxRoll;
            BaseMinRoll = System.Math.Min(minRoll, maxRoll);
            ActiveEffects = new List<StatusEffect>();
            mods = new ActiveModifiers();
            nextRollMods = new List<Modifier>();
        }

        // Get trigger chance from base with luck counted in.
        // Each positive luck point gives +5% (i.e. 3 luck is +15%,
        // -4 luck is -20%, etc.)
        public float GetTriggerChanceWithLuck(float baseTriggerChance)
        {
            return baseTriggerChance + (0.05f * Luck);
        }

        // Get all mods with roll bounded effects that apply to this actor
        public IEnumerable<Modifier> GetActiveRollBoundedBattleEffectModifiers()
        {
            return Mods.GetRollBoundedBattleEffectModifiers();
        }

        // Get mods for this actor that will apply next roll that have roll bounded effects
        public IEnumerable<Modifier> GetNextRollRollBoundedBattleEffectModifiers()
        {
            return NextRollMods.Where(m => m.battleEffect != RollBoundedBattleEffect.NONE);
        }
    }

    public enum StatusEffect
    {
        NONE,
        BREAK,
        POISON,
        ENRAGED
    }

    public static class StatusEffectExtensions
    {
        public static string Name(this StatusEffect effect)
        {
            switch (effect)
            {
                case StatusEffect.BREAK:
                    return "Break";
                case StatusEffect.POISON:
                    return "Poison";
                case StatusEffect.ENRAGED:
                    return "Enraged";
                default:
                    return null;
            }
        }

        public static Modifier Modifier(this StatusEffect effect)
        {
            switch (effect)
            {
                case StatusEffect.BREAK:
                    return new BreakModifier();
                case StatusEffect.POISON:
                    return new PoisonModifier();
                case StatusEffect.ENRAGED:
                    return new EnragedModifier();
                default:
                    return null;
            }
        }
    }
}
