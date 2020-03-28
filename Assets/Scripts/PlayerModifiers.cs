using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModifiers
{
    // Registered modifiers by priority
    private SortedDictionary<int, List<IRollGenerationModifier>> rollGenMods;
    private SortedDictionary<int, List<IRollResultModifier>> rollResultMods;
    private SortedDictionary<int, List<IRollValueModifier>> rollValueMods;

    public PlayerModifiers()
    {
        rollGenMods = new SortedDictionary<int, List<IRollGenerationModifier>>();
        rollResultMods = new SortedDictionary<int, List<IRollResultModifier>>();
        rollValueMods = new SortedDictionary<int, List<IRollValueModifier>>();
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

    public void RegisterModifier(Modifier mod, int priority)
    {
        bool registered = false;
        if (mod is IRollGenerationModifier)
        {
            registered = true;
            RegisterModifier((IRollGenerationModifier)mod, priority);
        }
        if (mod is IRollResultModifier)
        {
            registered = true;
            RegisterModifier((IRollResultModifier)mod, priority);
        }
        if (mod is IRollValueModifier)
        {
            registered = true;
            RegisterModifier((IRollValueModifier)mod, priority);
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

    public void DecrementAndDeregisterModsIfNecessary()
    {
        // Keep track of mods that have already been decremented so that
        // modifiers that implement multiple types don't get
        // decremented more than once.
        List<Modifier> decremented = new List<Modifier>();
        // Iterate over all mod lists to decrement them
        foreach (IRollGenerationModifier rollGenMod in GetRollGenerationModifiers())
        {
            Modifier mod = (Modifier)rollGenMod;
            if (!decremented.Contains(mod) && !DecrementAndDeregisterIfNecessary(mod))
            {
                decremented.Add(mod);
            }
        }
        foreach (IRollResultModifier rollResultMod in GetRollResultModifiers())
        {
            Modifier mod = (Modifier)rollResultMod;
            if (!decremented.Contains(mod) && !DecrementAndDeregisterIfNecessary(mod))
            {
                decremented.Add(mod);
            }
        }
        foreach (IRollValueModifier rollValueMod in GetRollValueModifiers())
        {
            Modifier mod = (Modifier)rollValueMod;
            if (!decremented.Contains(mod) && !DecrementAndDeregisterIfNecessary(mod))
            {
                decremented.Add(mod);
            }
        }
    }

    // Return true if the mod was deregistered by this method
    private static bool DecrementAndDeregisterIfNecessary(Modifier mod)
    {
        if (mod.isRollBounded)
        {
            mod.numRollsRemaining--;
            if (mod.numRollsRemaining <= 0)
            {
                mod.DeregisterSelf();
                return true;
            }
        }
        return false;
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
