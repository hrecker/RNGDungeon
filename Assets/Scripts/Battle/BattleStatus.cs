using Modifiers;

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

        public StatusEffect ActiveStatus { get; set; }

        private ActiveModifiers mods;
        public ActiveModifiers Mods { get { return mods; } }

        public BattleStatus(int maxHealth, int minRoll, int maxRoll)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            BaseMaxRoll = maxRoll;
            BaseMinRoll = System.Math.Min(minRoll, maxRoll);
            ActiveStatus = StatusEffect.NONE;
            mods = new ActiveModifiers();
        }

        // Get trigger chance from base with luck counted in.
        // Each positive luck point gives +5% (i.e. 3 luck is +15%,
        // -4 luck is -20%, etc.)
        public float GetTriggerChanceWithLuck(float baseTriggerChance)
        {
            return baseTriggerChance + (0.05f * Luck);
        }
    }

    public enum StatusEffect
    {
        NONE,
        BREAK
    }
}
