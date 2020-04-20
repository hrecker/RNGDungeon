using System.Collections.Generic;
using UnityEngine;
using Data;
using Modifiers;
using Levels;
using Battle;

public class PlayerStatus
{
    public static bool Initialized { get; set; }
    public static Vector3 MapPosition { get; set; }
    public static BattleStatus Status { get; set; }
    public static int KeyCount { get; set; }
    // Dictionary of item counts in inventory
    public static Dictionary<Item, int> Inventory { get; set; }

    private static Item equippedWeapon;
    private static Modifier weaponMod;
    public static Item EquippedWeapon { 
        get { return equippedWeapon; } 
        set
        {
            equippedWeapon = value;
            //TODO this will potentially be pretty inefficient in the inventory screen
            if (weaponMod != null)
            {
                weaponMod.DeregisterSelf();
            }
            if (equippedWeapon != null)
            {
                weaponMod = equippedWeapon.CreateItemModifier();
                Status.Mods.RegisterModifier(weaponMod);
            }
        }
    }
    public static List<Item> EquippedTrinkets { get; set; }
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
        Status = new BattleStatus(100, 1, 4);
        Status.Actor = BattleActor.PLAYER;
        MapPosition = CurrentLevel.GetPlayerStartingPosition();
        Inventory = new Dictionary<Item, int>();
        Inventory.Add(Data.Cache.GetItem("HealthPotion"), 3);
        Inventory.Add(Data.Cache.GetItem("BlockingPotion"), 1);
        Inventory.Add(Data.Cache.GetItem("RecoilPotion"), 1);
        Inventory.Add(Data.Cache.GetItem("Shortsword"), 1);
        abilities = new List<Ability>();
        EquippedTrinkets = new List<Item>();
        EquippedWeapon = Data.Cache.GetItem("Shortsword");
        EnabledTechs = new List<Tech>();
        EnabledTechs.Add(Data.Cache.GetTech("HeavySwing"));
        EnabledTechs.Add(Data.Cache.GetTech("Infect"));
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
        Modifier itemMod = null;
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
                        EquippedTrinkets.Add(item);
                        break;
                }
                break;
            case ItemType.USABLE_ANYTIME:
                itemMod = item.CreateItemModifier();
                break;
            case ItemType.USABLE_ONLY_IN_BATTLE:
                if (isInBattle)
                {
                    itemMod = item.CreateItemModifier();
                }
                else
                {
                    return false;
                }
                break;
        }

        if (itemMod != null)
        {
            // If this is a one time mod and it can't be applied, return false
            if (itemMod is IOneTimeEffectModifier && 
                !((IOneTimeEffectModifier)itemMod).CanApply())
            {
                    return false;
            }
            Status.Mods.RegisterModifier(itemMod);
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

    public static void AddAbility(Ability ability)
    {
        abilities.Add(ability);
        Status.Mods.RegisterModifier(ability.CreateAbilityModifier());
    }
}
