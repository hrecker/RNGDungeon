using System;
using Battle.Enemies;

namespace Data
{
    [Serializable]
    public class Enemy
    {
        public string name;
        public int baseMinRoll;
        public int baseMaxRoll;
        public int maxHealth;

        public Type GetEnemyControllerType()
        {
            switch (name)
            {
                case "Bat":
                    return typeof(EnemyBattleController);
                case "Collector":
                    return typeof(CollectorBattleController);
                case "Slime":
                    return typeof(SlimeBattleController);
                case "Snake":
                    return typeof(SnakeBattleController);
                case "Ghost":
                    return typeof(GhostBattleController);
                case "MysteriousStatue":
                    return typeof(MysteriousStatueBattleController);
                case "PossessedAxe":
                    return typeof(PossessedAxeBattleController);
                case "FireBug":
                    return typeof(FireBugBattleController);
                case "Spirit":
                    return typeof(SpiritBattleController);
                case "TauntingFairy":
                    return typeof(TauntingFairyBattleController);
                case "Tortoise":
                    return typeof(TortoiseBattleController);
                case "Hare":
                    return typeof(HareBattleController);
                case "Robot":
                    return typeof(RobotBattleController);
                case "WolfPack":
                    return typeof(WolfPackBattleController);
                case "Ent":
                    return typeof(EntBattleController);
                case "Witch":
                    return typeof(WitchBattleController);
                case "Giant":
                    return typeof(GiantBattleController);
                case "Boss":
                    return typeof(BossBattleController);
                default:
                    return typeof(EnemyBattleController);
            }
        }
    }
}
