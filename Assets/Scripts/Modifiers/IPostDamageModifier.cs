// Modifiers causing an effect after damage is done
public interface IPostDamageModifier
{
    void ApplyPostDamageMod(RollResult rollResult);
}
