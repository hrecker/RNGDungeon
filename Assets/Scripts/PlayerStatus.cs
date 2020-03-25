using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    public static Vector3 MapPosition { get; set; }
    public static int MaxHealth { get; set; }
    public static int Health { get; set; }
    public static string SelectedStance { get; set; }
    public static bool Initialized { get; set; }
    // Dictionary of item counts in inventory
    public static Dictionary<Item, int> Inventory { get; set; }

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
        MapPosition = CurrentLevel.GetPlayerStartingPosition();
        SelectedStance = "Neutral";
        Inventory = new Dictionary<Item, int>();
        Inventory.Add(Cache.GetItem("HealthPotion"), 3);
        Inventory.Add(Cache.GetItem("BlockingPotion"), 1);
        Inventory.Add(Cache.GetItem("RecoilPotion"), 1);
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
}
