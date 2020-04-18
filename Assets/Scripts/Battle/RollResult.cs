namespace Battle
{
    public class RollResult
    {
        public int PlayerDamage { get; set; }
        public int PlayerHeal { get; set; }
        public int EnemyDamage { get; set; }
        public int EnemyHeal { get; set; }

        public void SetDamage(BattleActor actor, int damage)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    PlayerDamage = damage;
                    break;
                case BattleActor.ENEMY:
                    EnemyDamage = damage;
                    break;
                default:
                    throw new System.Exception("Invalid battle actor type");
            }
        }

        public void SetHeal(BattleActor actor, int heal)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    PlayerHeal = heal;
                    break;
                case BattleActor.ENEMY:
                    EnemyHeal = heal;
                    break;
                default:
                    throw new System.Exception("Invalid battle actor type");
            }
        }
        public int GetTotalPlayerHealthChange()
        {
            return PlayerHeal - PlayerDamage;
        }

        public int GetTotalEnemyHealthChange()
        {
            return EnemyHeal - EnemyDamage;
        }

        public int GetDamage(BattleActor actor)
        {
            switch(actor)
            {
                case BattleActor.PLAYER:
                    return PlayerDamage;
                case BattleActor.ENEMY:
                    return EnemyDamage;
                default:
                    throw new System.Exception("Invalid battle actor type");
            }
        }

        public int GetHeal(BattleActor actor)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    return PlayerHeal;
                case BattleActor.ENEMY:
                    return EnemyHeal;
                default:
                    throw new System.Exception("Invalid battle actor type");
            }
        }

        public int GetTotalHealthChange(BattleActor actor)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    return GetTotalPlayerHealthChange();
                case BattleActor.ENEMY:
                    return GetTotalEnemyHealthChange();
                default:
                    throw new System.Exception("Invalid battle actor type");
            }
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
