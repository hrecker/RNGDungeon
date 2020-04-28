namespace Battle
{
    public class RollGeneration
    {
        public int MinRoll { get; set; }
        public int MaxRoll { get; set; }
        // How many rolls have passed in battle so far
        public int CurrentRoll { get; set; }
        public Data.Tech PlayerTech { get; set; }
    }
}
