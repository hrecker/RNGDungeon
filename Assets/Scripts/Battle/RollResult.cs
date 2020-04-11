namespace Battle
{
    public class RollResult
    {
        public int PlayerDamage { get; set; }
        public int PlayerHeal { get; set; }
        public int EnemyDamage { get; set; }
        public int EnemyHeal { get; set; }
        //TODO status effects, etc.

        public int GetTotalPlayerHealthChange()
        {
            return PlayerHeal - PlayerDamage;
        }

        public int GetTotalEnemyHealthChange()
        {
            return EnemyHeal - EnemyDamage;
        }

        public override string ToString()
        {
            return base.ToString() + ": Player damage: " + PlayerDamage +
                ", Player heal: " + PlayerHeal +
                ", Enemy damage: " + EnemyDamage +
                ", Enemy heal: " + EnemyHeal;
        }
    }
}
