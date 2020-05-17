using Data;
using Levels;
using System.Collections.Generic;

namespace Modifiers.Ability
{
    // Give the player a random rare item
    public class RelicModifier : Modifier, IOneTimeEffectModifier
    {
        public void ApplyOneTimeEffectMod()
        {
            Dictionary<Rarity, float> alwaysRare = new Dictionary<Rarity, float>()
            {
                { Rarity.ULTRARARE, 1.0f }
            };
            PlayerController.randomlyGivenItem = Data.Cache.GetRandomItem(alwaysRare);
        }

        public bool CanApply()
        {
            return true;
        }
    }
}
