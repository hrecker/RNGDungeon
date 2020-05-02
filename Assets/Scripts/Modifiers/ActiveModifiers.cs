using System;
using System.Collections.Generic;
using System.Linq;

namespace Modifiers
{
    public class ActiveModifiers
    {
        // Registered modifiers by priority
        private SortedDictionary<int, List<IRollGenerationModifier>> rollGenMods;
        private SortedDictionary<int, List<IRollResultModifier>> rollResultMods;
        private SortedDictionary<int, List<IRollValueModifier>> rollValueMods;
        private SortedDictionary<int, List<IPostDamageModifier>> postDamageMods;
        private SortedDictionary<int, List<IPostBattleModifier>> postBattleMods;
        private SortedDictionary<int, List<ITechModifier>> techMods;

        private List<Modifier> uniqueRegisteredModifiers;

        public ActiveModifiers()
        {
            rollGenMods = new SortedDictionary<int, List<IRollGenerationModifier>>();
            rollResultMods = new SortedDictionary<int, List<IRollResultModifier>>();
            rollValueMods = new SortedDictionary<int, List<IRollValueModifier>>();
            postDamageMods = new SortedDictionary<int, List<IPostDamageModifier>>();
            postBattleMods = new SortedDictionary<int, List<IPostBattleModifier>>();
            techMods = new SortedDictionary<int, List<ITechModifier>>();
            uniqueRegisteredModifiers = new List<Modifier>();
        }

        public IEnumerable<Modifier> GetRollBoundedBattleEffectModifiers()
        {
            return uniqueRegisteredModifiers.Where(m => 
                m.battleEffect != RollBoundedBattleEffect.NONE);
        }

        public List<IRollGenerationModifier> GetRollGenerationModifiers()
        {
            return GetModifiers(rollGenMods);
        }

        public List<IRollResultModifier> GetRollResultModifiers()
        {
            return GetModifiers(rollResultMods);
        }

        public List<IRollValueModifier> GetRollValueModifiers()
        {
            return GetModifiers(rollValueMods);
        }

        public List<IPostDamageModifier> GetPostDamageModifiers()
        {
            return GetModifiers(postDamageMods);
        }

        public List<IPostBattleModifier> GetPostBattleModifiers()
        {
            return GetModifiers(postBattleMods);
        }

        public List<ITechModifier> GetTechModifiers()
        {
            return GetModifiers(techMods);
        }

        public void RegisterModifier(Modifier mod)
        {
            // When attempting to register a status effect, don't allow stacking.
            // Just extend the existing status effect modifier
            if (mod.statusEffect != Battle.StatusEffect.NONE)
            {
                foreach (Modifier existingMod in uniqueRegisteredModifiers)
                {
                    if (mod.statusEffect == existingMod.statusEffect)
                    {
                        existingMod.numRollsRemaining = Math.Max(mod.numRollsRemaining,
                            existingMod.numRollsRemaining);
                        return;
                    }
                }
            }

            bool registered = false;
            if (mod is IRollGenerationModifier)
            {
                registered = true;
                RegisterModifier((IRollGenerationModifier)mod, mod.priority);
            }
            if (mod is IRollResultModifier)
            {
                registered = true;
                RegisterModifier((IRollResultModifier)mod, mod.priority);
            }
            if (mod is IRollValueModifier)
            {
                registered = true;
                RegisterModifier((IRollValueModifier)mod, mod.priority);
            }
            if (mod is IPostDamageModifier)
            {
                registered = true;
                RegisterModifier((IPostDamageModifier)mod, mod.priority);
            }
            if (mod is IPostBattleModifier)
            {
                registered = true;
                RegisterModifier((IPostBattleModifier)mod, mod.priority);
            }
            if (mod is ITechModifier)
            {
                registered = true;
                RegisterModifier((ITechModifier)mod, mod.priority);
            }

            // One shot modifiers are usually just instantly applied and don't need to be registered
            // However if they are roll bounded they are registered so they can be deregistered
            // after the right number of rolls.
            if (mod is IOneTimeEffectModifier)
            {
                registered = true;
                ((IOneTimeEffectModifier)mod).ApplyOneTimeEffectMod();
                if (mod.isRollBounded)
                {
                    uniqueRegisteredModifiers.Add(mod);
                }
            }
            else if (registered)
            {
                uniqueRegisteredModifiers.Add(mod);
            }

            if (!registered)
            {
                throw new System.Exception("Invalid modification type?");
            }
        }

        private void RegisterModifier(IRollGenerationModifier mod, int priority)
        {
            RegisterModifier(rollGenMods, mod, priority);
        }

        private void RegisterModifier(IRollResultModifier mod, int priority)
        {
            RegisterModifier(rollResultMods, mod, priority);
        }

        private void RegisterModifier(IRollValueModifier mod, int priority)
        {
            RegisterModifier(rollValueMods, mod, priority);
        }

        private void RegisterModifier(IPostDamageModifier mod, int priority)
        {
            RegisterModifier(postDamageMods, mod, priority);
        }

        private void RegisterModifier(IPostBattleModifier mod, int priority)
        {
            RegisterModifier(postBattleMods, mod, priority);
        }

        private void RegisterModifier(ITechModifier mod, int priority)
        {
            RegisterModifier(techMods, mod, priority);
        }

        public bool DeregisterModifier(Modifier mod)
        {
            bool deregistered = false;
            if (mod is IRollGenerationModifier)
            {
                deregistered |= DeregisterModifier((IRollGenerationModifier)mod);
            }
            if (mod is IRollResultModifier)
            {
                deregistered |= DeregisterModifier((IRollResultModifier)mod);
            }
            if (mod is IRollValueModifier)
            {
                deregistered |= DeregisterModifier((IRollValueModifier)mod);
            }
            if (mod is IPostDamageModifier)
            {
                deregistered |= DeregisterModifier((IPostDamageModifier)mod);
            }
            if (mod is IPostBattleModifier)
            {
                deregistered |= DeregisterModifier((IPostBattleModifier)mod);
            }
            if (mod is ITechModifier)
            {
                deregistered |= DeregisterModifier((ITechModifier)mod);
            }
            uniqueRegisteredModifiers.Remove(mod);
            return deregistered;
        }

        private bool DeregisterModifier(IRollGenerationModifier mod)
        {
            return DeregisterModifier(rollGenMods, mod);
        }

        private bool DeregisterModifier(IRollResultModifier mod)
        {
            return DeregisterModifier(rollResultMods, mod);
        }

        private bool DeregisterModifier(IRollValueModifier mod)
        {
            return DeregisterModifier(rollValueMods, mod);
        }

        private bool DeregisterModifier(IPostDamageModifier mod)
        {
            return DeregisterModifier(postDamageMods, mod);
        }

        private bool DeregisterModifier(IPostBattleModifier mod)
        {
            return DeregisterModifier(postBattleMods, mod);
        }

        private bool DeregisterModifier(ITechModifier mod)
        {
            return DeregisterModifier(techMods, mod);
        }

        public void DecrementAndDeregisterModsIfNecessary()
        {
            foreach (Modifier mod in uniqueRegisteredModifiers.ToList())
            {
                DecrementAndDeregisterIfNecessary(mod);
            }
        }

        // Return true if the mod was deregistered by this method
        private static bool DecrementAndDeregisterIfNecessary(Modifier mod)
        {
            if (mod.isRollBounded && !mod.isFullBattleModifier)
            {
                if (mod.numRollsRemaining > 0)
                {
                    mod.numRollsRemaining--;
                }
                if (mod.numRollsRemaining <= 0)
                {
                    mod.DeregisterSelf();
                    return true;
                }
            }
            return false;
        }

        public void DeregisterAllRollBoundedMods()
        {
            for (int i = uniqueRegisteredModifiers.Count - 1; i >= 0; i--)
            {
                if (uniqueRegisteredModifiers[i].isRollBounded ||
                    uniqueRegisteredModifiers[i].isFullBattleModifier)
                {
                    uniqueRegisteredModifiers[i].DeregisterSelf();
                }
            }
        }

        private static List<T> GetModifiers<T>(SortedDictionary<int, List<T>> registered)
        {
            List<T> result = new List<T>();
            if (registered != null)
            {
                foreach (List<T> mods in registered.Values)
                {
                    foreach (T mod in mods)
                    {
                        result.Add(mod);
                    }
                }
            }
            return result;
        }

        private static void RegisterModifier<T>(SortedDictionary<int, List<T>> registered,
            T mod, int priority)
        {
            if (!registered.ContainsKey(priority))
            {
                registered.Add(priority, new List<T>());
            }
            registered[priority].Add(mod);
        }

        private static bool DeregisterModifier<T>(SortedDictionary<int, List<T>> registered, T mod)
        {
            bool removed = false;
            if (registered != null)
            {
                foreach (List<T> mods in registered.Values)
                {
                    removed |= mods.Remove(mod);
                    if (removed)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
