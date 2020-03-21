public class RollResult
{
    // Positive indicates damage done, negative indicates health gained
    public int PlayerDamage { get; set; }
    public int EnemyDamage { get; set; }
    //TODO status effects, etc.
    public override string ToString()
    {
        return base.ToString() + ": Player damage: " + 
            PlayerDamage + ", Enemy damage: " + EnemyDamage;
    }
}
