namespace Battle
{
    public class RollResult
    {
        public int PlayerRollDamage { get; set; }
        public int PlayerNonRollDamage { get; set; }
        public int PlayerDamage
        { 
            get { return PlayerRollDamage + PlayerNonRollDamage; } 
        }
        public int PlayerHeal { get; set; }
        public int EnemyRollDamage { get; set; }
        public int EnemyNonRollDamage { get; set; }
        public int EnemyDamage
        {
            get { return EnemyRollDamage + EnemyNonRollDamage; }
        }
        public int EnemyHeal { get; set; }

        public void SetRollDamage(BattleActor actor, int damage)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    PlayerRollDamage = damage;
                    break;
                case BattleActor.ENEMY:
                    EnemyRollDamage = damage;
                    break;
                default:
                    throw new System.Exception("Invalid battle actor type");
            }
        }

        public void SetNonRollDamage(BattleActor actor, int damage)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    PlayerNonRollDamage = damage;
                    break;
                case BattleActor.ENEMY:
                    EnemyNonRollDamage = damage;
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

        public void AddRollDamage(BattleActor actor, int diff)
        {
            SetRollDamage(actor, GetRollDamage(actor) + diff);
        }

        public void AddNonRollDamage(BattleActor actor, int diff)
        {
            SetNonRollDamage(actor, GetNonRollDamage(actor) + diff);
        }

        public void AddHeal(BattleActor actor, int diff)
        {
            SetHeal(actor, GetHeal(actor) + diff);
        }

        public int GetTotalPlayerHealthChange()
        {
            return PlayerHeal - (PlayerRollDamage + PlayerNonRollDamage);
        }

        public int GetTotalEnemyHealthChange()
        {
            return EnemyHeal - (EnemyRollDamage + EnemyNonRollDamage);
        }

        public int GetRollDamage(BattleActor actor)
        {
            switch(actor)
            {
                case BattleActor.PLAYER:
                    return PlayerRollDamage;
                case BattleActor.ENEMY:
                    return EnemyRollDamage;
                default:
                    throw new System.Exception("Invalid battle actor type");
            }
        }

        public int GetNonRollDamage(BattleActor actor)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    return PlayerNonRollDamage;
                case BattleActor.ENEMY:
                    return EnemyNonRollDamage;
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
            return base.ToString() + 
                ": Player roll damage: " + PlayerRollDamage +
                ", Player non-roll damage: " + PlayerNonRollDamage +
                ", Player heal: " + PlayerHeal +
                ", Enemy roll damage: " + EnemyRollDamage +
                ", Enemy non-roll damage: " + EnemyNonRollDamage +
                ", Enemy heal: " + EnemyHeal;
        }
    }
}
