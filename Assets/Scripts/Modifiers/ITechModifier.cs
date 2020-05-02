namespace Modifiers
{
    public interface ITechModifier
    {
        int ApplyTechCooldownModifier(Data.Tech tech, bool isSelectedTech, int startingCooldown);
    }
}
