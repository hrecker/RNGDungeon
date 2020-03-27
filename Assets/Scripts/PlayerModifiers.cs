using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModifiers
{
    // Registered modifiers by priority
    private SortedDictionary<int, List<RollGenerationModifier>> rollGenMods;
    private SortedDictionary<int, List<RollResultModifier>> rollResultMods;
    private SortedDictionary<int, List<RollValueModifier>> rollValueMods;

    public PlayerModifiers()
    {
        rollGenMods = new SortedDictionary<int, List<RollGenerationModifier>>();
        rollResultMods = new SortedDictionary<int, List<RollResultModifier>>();
        rollValueMods = new SortedDictionary<int, List<RollValueModifier>>();
    }

    public List<RollGenerationModifier> GetRollGenerationModifiers()
    {
        return GetModifiers(rollGenMods);
    }

    public List<RollResultModifier> GetRollResultModifiers()
    {
        return GetModifiers(rollResultMods);
    }

    public List<RollValueModifier> GetRollValueModifiers()
    {
        return GetModifiers(rollValueMods);
    }

    public void RegisterModifier(Modifier mod, int priority)
    {
        if (mod is RollGenerationModifier)
        {
            RegisterModifier((RollGenerationModifier)mod, priority);
        }
        else if (mod is RollResultModifier)
        {
            RegisterModifier((RollResultModifier)mod, priority);
        }
        else if (mod is RollValueModifier)
        {
            RegisterModifier((RollValueModifier)mod, priority);
        }
        else
        {
            throw new System.Exception("Invalid modification type?");
        }
    }

    public void RegisterModifier(RollGenerationModifier mod, int priority)
    {
        RegisterModifier(rollGenMods, mod, priority);
    }

    public void RegisterModifier(RollResultModifier mod, int priority)
    {
        RegisterModifier(rollResultMods, mod, priority);
    }

    public void RegisterModifier(RollValueModifier mod, int priority)
    {
        RegisterModifier(rollValueMods, mod, priority);
    }

    public bool DeregisterModifier(Modifier mod)
    {
        if (mod is RollGenerationModifier)
        {
            return DeregisterModifier((RollGenerationModifier)mod);
        }
        else if (mod is RollResultModifier)
        {
            return DeregisterModifier((RollResultModifier)mod);
        }
        else if (mod is RollValueModifier)
        {
            return DeregisterModifier((RollValueModifier)mod);
        }
        else
        {
            throw new System.Exception("Invalid modification type?");
        }
    }

    public bool DeregisterModifier(RollGenerationModifier mod)
    {
        return DeregisterModifier(rollGenMods, mod);
    }

    public bool DeregisterModifier(RollResultModifier mod)
    {
        return DeregisterModifier(rollResultMods, mod);
    }

    public bool DeregisterModifier(RollValueModifier mod)
    {
        return DeregisterModifier(rollValueMods, mod);
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
