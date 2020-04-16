namespace Modifiers
{
    // Modifiers that have some effect after a battle is completed (won) by the player
    public interface IPostBattleModifier
    {
        void ApplyPostBattleMod();
    }
}
