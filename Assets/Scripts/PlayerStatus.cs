using System.Collections.Generic;
using UnityEngine;
using Data;
using Modifiers;
using Levels;
using Battle;
using System;

public class PlayerStatus
{
    public static bool Initialized { get; set; }
    public static Vector3 MapPosition { get; set; }
    public static BattleStatus Status { get; set; }
    public static int KeyCount { get; set; }
    // Dictionary of item counts in inventory
    public static Dictionary<Item, int> Inventory { get; set; }

    private static Item equippedWeapon;
    private static List<Modifier> weaponMods;
    public static Item EquippedWeapon { 
        get { return equippedWeapon; } 
        set
        {
            equippedWeapon = value;
            //TODO this will potentially be pretty inefficient in the inventory screen
            if (weaponMods.Count > 0)
            {
                foreach (Modifier mod in weaponMods)
                {
                    mod.DeregisterSelf();
                }
                weaponMods.Clear();
            }
            if (equippedWeapon != null)
            {
                foreach (Modifier weaponMod in equippedWeapon.CreateItemModifiers())
                {
                    weaponMods.Add(weaponMod);
                    Status.Mods.RegisterModifier(weaponMod);
                }
            }
        }
    }
    // Trinkets currently equipped, each with an assigned ID
    public static Dictionary<Item, List<Guid>> EquippedTrinketIds { get; set; }
    // Modifiers identified by trinket ID
    private static Dictionary<Guid, List<Modifier>> trinketModifiers;
    private static List<Ability> abilities;
    public static List<Tech> EnabledTechs { get; set; }

    public static void InitializeIfNecessary()
    {
        if (!Initialized)
        {
            Restart();
        }
    }

    public static void Restart()
    {
        Status = new BattleStatus(300, 1, 4);
        Status.Actor = BattleActor.PLAYER;
        MapPosition = CurrentLevel.GetPlayerStartingPosition();
        Inventory = new Dictionary<Item, int>();
        Inventory.Add(Data.Cache.GetItem("HealthPotion"), 3);
        Inventory.Add(Data.Cache.GetItem("Panacea"), 2);
        abilities = new List<Ability>();
        weaponMods = new List<Modifier>();
        EquippedTrinketIds = new Dictionary<Item, List<Guid>>();
        trinketModifiers = new Dictionary<Guid, List<Modifier>>();
        EnabledTechs = new List<Tech>();
        Initialized = true;
    }

    public static void AddItem(Item item)
    {
        int currentCount;
        if (Inventory.TryGetValue(item, out currentCount))
        {
            Inventory[item] = currentCount + 1;
        }
        else
        {
            Inventory.Add(item, 1);
        }
    }

    // Returns true if the item was successfully used
    public static bool UseItem(Item item, bool isInBattle)
    {
        List<Modifier> itemMods = new List<Modifier>();
        switch (item.itemType)
        {
            case ItemType.EQUIPMENT:
                //Equip
                switch (item.equipSlot)
                {
                    case EquipSlot.WEAPON:
                        Item currentWeapon = EquippedWeapon;
                        if (currentWeapon != null)
                        {
                            AddItem(currentWeapon);
                        }
                        EquippedWeapon = item;
                        break;
                    case EquipSlot.TRINKET:
                        EquipTrinket(item);
                        break;
                }
                break;
            case ItemType.USABLE_ANYTIME:
                itemMods.AddRange(item.CreateItemModifiers());
                break;
            case ItemType.USABLE_ONLY_IN_BATTLE:
                if (isInBattle)
                {
                    itemMods.AddRange(item.CreateItemModifiers());
                }
                else
                {
                    return false;
                }
                break;
        }

        if (itemMods.Count > 0)
        {
            // If there is a one time mod and it can't be applied, return false
            foreach (Modifier itemMod in itemMods)
            {
                if (itemMod is IOneTimeEffectModifier &&
                    !((IOneTimeEffectModifier)itemMod).CanApply())
                {
                    return false;
                }
            }
            // Otherwise register all item mods
            foreach (Modifier itemMod in itemMods)
            {
                Status.Mods.RegisterModifier(itemMod);
            }
        }

        if (Inventory.ContainsKey(item))
        {
            Inventory[item]--;
        }
        if (Inventory[item] <= 0)
        {
            Inventory.Remove(item);
        }

        return true;
    }

    public static List<Ability> GetAbilities()
    {
        return new List<Ability>(abilities);
    }

    public static List<Tech> GetTechs()
    {
        return new List<Tech>(EnabledTechs);
    }

    public static void AddAbility(Ability ability)
    {
        abilities.Add(ability);
        Status.Mods.RegisterModifier(ability.CreateAbilityModifier());
    }

    public static void AddTech(Tech tech)
    {
        tech.ResetCooldown();
        EnabledTechs.Add(tech);
    }

    private static void EquipTrinket(Item trinket)
    {
        Guid trinketID = Guid.NewGuid();
        if (!EquippedTrinketIds.ContainsKey(trinket))
        {
            EquippedTrinketIds.Add(trinket, new List<Guid>());
        }
        EquippedTrinketIds[trinket].Add(trinketID);
        trinketModifiers.Add(trinketID, trinket.CreateItemModifiers());
        foreach (Modifier mod in trinketModifiers[trinketID])
        {
            Status.Mods.RegisterModifier(mod);
        }
    }

    public static void UnequipTrinket(Item trinket)
    {
        List<Guid> trinketIDs = EquippedTrinketIds[trinket];
        Guid trinketID = trinketIDs[0];
        trinketIDs.Remove(trinketID);
        if (trinketIDs.Count == 0)
        {
            EquippedTrinketIds.Remove(trinket);
        }
        foreach (Modifier mod in trinketModifiers[trinketID])
        {
            mod.DeregisterSelf();
        }
        trinketModifiers.Remove(trinketID);
    }
}
