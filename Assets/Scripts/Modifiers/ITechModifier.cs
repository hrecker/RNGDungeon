namespace Modifiers
{
    public interface ITechModifier
    {
        int ApplyTechCooldownModifier(Data.Tech tech, int startingCooldown);
    }
}
