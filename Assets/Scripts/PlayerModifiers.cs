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

    public void RegisterModifier(IRollGenerationModifier mod, int priority)
    {
        RegisterModifier(rollGenMods, mod, priority);
    }

    public void RegisterModifier(IRollResultModifier mod, int priority)
    {
        RegisterModifier(rollResultMods, mod, priority);
    }

    public void RegisterModifier(IRollValueModifier mod, int priority)
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

    public bool DeregisterModifier(IRollGenerationModifier mod)
    {
        return DeregisterModifier(rollGenMods, mod);
    }

    public bool DeregisterModifier(IRollResultModifier mod)
    {
        return DeregisterModifier(rollResultMods, mod);
    }

    public bool DeregisterModifier(IRollValueModifier mod)
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
