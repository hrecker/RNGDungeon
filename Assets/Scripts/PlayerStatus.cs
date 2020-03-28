using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
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
    public static bool Initialized { get; set; }
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
                Mods.RegisterModifier(weaponMod, equippedWeapon.modEffect.modPriority);
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
        MaxHealth = 10;
        Health = MaxHealth;
        Mods = new PlayerModifiers();
        MapPosition = CurrentLevel.GetPlayerStartingPosition();
        Inventory = new Dictionary<Item, int>();
        Inventory.Add(Cache.GetItem("HealthPotion"), 3);
        Inventory.Add(Cache.GetItem("BlockingPotion"), 1);
        Inventory.Add(Cache.GetItem("RecoilPotion"), 1);
        Inventory.Add(Cache.GetItem("Shortsword"), 1);
        abilities = new List<Ability>();
        EquippedTrinkets = new List<Item>();
        EquippedWeapon = Cache.GetItem("Shortsword");
        EnabledTechs = new List<Tech>();
        EnabledTechs.Add(Cache.GetTech("HeavySwing"));
        EnabledTechs.Add(Cache.GetTech("Bulwark"));
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

    public static void UseItem(Item item)
    {
        if (Inventory.ContainsKey(item))
        {
            Inventory[item]--;
        }
        if (Inventory[item] <= 0)
        {
            Inventory.Remove(item);
        }
    }

    public static List<Ability> GetAbilities()
    {
        return new List<Ability>(abilities);
    }

    public static void AddAbility(Ability ability)
    {
        abilities.Add(ability);
        Mods.RegisterModifier(ability.CreateAbilityModifier(), 
            ability.modEffect.modPriority);
    }
}
