using System.Collections.Generic;
using UnityEngine;
using Data;
using Modifiers;
using Levels;

public class PlayerStatus
{
    public static bool Initialized { get; set; }
    public static Vector3 MapPosition { get; set; }
    public static int MaxHealth { get; set; }
    private static int health;
    public static int Health { get { return health; } 
        set
        {
            health = value;
            if (health < 0)
            {
                health = 0;
            }
            else if (health > MaxHealth)
            {
                health = MaxHealth;
            }
        }
    }
    public static int BaseMinRoll { get; set; }
    public static int BaseMaxRoll { get; set; }
    public static int Luck { get; set; }
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
                Mods.DeregisterModifier(weaponMod);
            }
            if (equippedWeapon != null)
            {
                weaponMod = equippedWeapon.CreateItemModifier();
                Mods.RegisterModifier(weaponMod);
            }
        }
    }
    public static List<Item> EquippedTrinkets { get; set; }
    private static List<Ability> abilities;
    public static List<Tech> EnabledTechs { get; set; }

    public static PlayerModifiers Mods { get; private set; }

    public static void InitializeIfNecessary()
    {
        if (!Initialized)
        {
            Restart();
        }
    }

    public static void Restart()
    {
        MaxHealth = 100;
        Health = MaxHealth;
        BaseMinRoll = 1;
        BaseMaxRoll = 4;
        Luck = 0;
        Mods = new PlayerModifiers();
        MapPosition = CurrentLevel.GetPlayerStartingPosition();
        Inventory = new Dictionary<Item, int>();
        Inventory.Add(Data.Cache.GetItem("HealthPotion"), 3);
        Inventory.Add(Data.Cache.GetItem("BlockingPotion"), 1);
        Inventory.Add(Data.Cache.GetItem("RecoilPotion"), 1);
        Inventory.Add(Data.Cache.GetItem("Shortsword"), 1);
        abilities = new List<Ability>();
        AddAbility(Data.Cache.GetAbility("LuckyHorseshoe"));
        AddAbility(Data.Cache.GetAbility("LuckyHorseshoe"));
        AddAbility(Data.Cache.GetAbility("LuckyHorseshoe"));
        AddAbility(Data.Cache.GetAbility("HighRoller"));
        EquippedTrinkets = new List<Item>();
        EquippedWeapon = Data.Cache.GetItem("Shortsword");
        EnabledTechs = new List<Tech>();
        EnabledTechs.Add(Data.Cache.GetTech("HeavySwing"));
        EnabledTechs.Add(Data.Cache.GetTech("Bulwark"));
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
            Mods.RegisterModifier(itemMod);
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
        Mods.RegisterModifier(ability.CreateAbilityModifier());
    }

    // Get trigger chance from base with luck counted in.
    // Each positive luck point gives +5% (i.e. 3 luck is +15%,
    // -4 luck is -20%, etc.)
    public static float GetTriggerChanceWithLuck(float baseTriggerChance)
    {
        return baseTriggerChance + (0.05f * Luck);
    }
}
