using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Caches configuration used for game
public class Cache
{
    private static Dictionary<int, Level> levels;
    private static Dictionary<string, Item> items;
    private static Dictionary<string, Enemy> enemies;
    private static Dictionary<string, Ability> abilities;
    private static Dictionary<string, Sprite> itemIcons;

    private static string levelsJsonPath = @"Levels/levels";
    private static string itemsJsonPath = @"Items/items";
    private static string enemiesJsonPath = @"Enemies/enemies";
    private static string abilitiesJsonPath = @"Abilities/abilities";

    private static bool initialized;

    public static void Load()
    {
        if (!initialized)
        {
            Reload();
        }
    }

    public static void Reload()
    {
        initialized = true;

        levels = new Dictionary<int, Level>();
        items = new Dictionary<string, Item>();
        enemies = new Dictionary<string, Enemy>();
        abilities = new Dictionary<string, Ability>();
        itemIcons = new Dictionary<string, Sprite>();

        // Load JSON from resouce files
        TextAsset levelsFile = Resources.Load<TextAsset>(levelsJsonPath);
        TextAsset itemsFile = Resources.Load<TextAsset>(itemsJsonPath);
        TextAsset enemiesFile = Resources.Load<TextAsset>(enemiesJsonPath);
        TextAsset abilitiesFile = Resources.Load<TextAsset>(abilitiesJsonPath);

        Levels levelsContainer = JsonUtility.FromJson<Levels>(levelsFile.text);
        Items itemsContainer = JsonUtility.FromJson<Items>(itemsFile.text);
        Enemies enemiesContainer = JsonUtility.FromJson<Enemies>(enemiesFile.text);
        Abilities abilitiesContainer = JsonUtility.FromJson<Abilities>(abilitiesFile.text);

        foreach (Level level in levelsContainer.levels)
        {
            levels.Add(level.floor, level);
        }
        foreach (Item item in itemsContainer.items)
        {
            items.Add(item.name, item);
            itemIcons.Add(item.name, Resources.Load<Sprite>(@"Items/sprites/" + item.name));
        }
        foreach (Enemy enemy in enemiesContainer.enemies)
        {
            enemies.Add(enemy.name, enemy);
        }
        foreach (Ability ability in abilitiesContainer.abilities)
        {
            abilities.Add(ability.name, ability);
        }
    }

    public static Level GetLevel(int floor)
    {
        Load();
        if (!levels.ContainsKey(floor))
        {
            return null;
        }
        return levels[floor];
    }

    public static Item GetItem(string name)
    {
        Load();
        return items[name];
    }

    public static Sprite GetItemIcon(string name)
    {
        Load();
        return itemIcons[name];
    }

    public static Enemy GetEnemy(string name)
    {
        Load();
        return enemies[name];
    }

    public static Ability GetAbility(string name)
    {
        Load();
        return abilities[name];
    }


    // Only to be used while developing - can't serialize to the resources
    // folder in the built game
    public static void SerializeExample()
    {
        Reload();
        Levels levelContainer = new Levels() { levels = new List<Level>() };
        Items itemContainer = new Items() { items = new List<Item>() };
        Enemies enemyContainer = new Enemies() { enemies = new List<Enemy>() };
        Abilities abilityContainer = new Abilities() { abilities = new List<Ability>() };

        // Sample levels
        levelContainer.levels.Add(new Level()
        {
            floor = 1,
            height = 10,
            width = 10,
            encounterRate = 0.1f,
            enemies = new string[] { "Bat", "Slime" },
            enemyEncounterRates = new float[] { 0.5f, 0.5f }
        });
        levelContainer.levels.Add(new Level()
        {
            floor = 2,
            height = 15,
            width = 10,
            encounterRate = 0.15f,
            enemies = new string[] { "Bat", "Slime" },
            enemyEncounterRates = new float[] { 0.7f, 0.3f }
        });
        // Sample items
        itemContainer.items.Add(new Item() 
        { 
            name = "HealthPotion", 
            itemEffect = new ItemEffect() 
            { 
                playerHealthChange = 3
            } 
        });
        itemContainer.items.Add( new Item() 
        { 
            name = "BlockingPotion", 
            itemEffect = new ItemEffect() 
            { 
                rollBoundedEffect = RollBoundedEffect.BLOCK,
                numRollsInEffect = 3
            }
        });
        // Sample enemies
        enemyContainer.enemies.Add(new Enemy()
        {
            name = "Bat",
            maxHealth = 5,
            baseMinRoll = 1,
            baseMaxRoll = 3,
        });
        enemyContainer.enemies.Add(new Enemy()
        {
            name = "Bat",
            maxHealth = 4,
            baseMinRoll = 2,
            baseMaxRoll = 2,
        });
        // Sample abilities
        abilityContainer.abilities.Add(new Ability() { name = "Stalwart" });
        abilityContainer.abilities.Add(new Ability() { name = "HighRoller" });

        string outputPath = @"C:\Users\henry\SampleSerializedJson\";
        string levelJsonPath = outputPath + "levels.json";
        string itemJsonPath = outputPath + "items.json";
        string enemyJsonPath = outputPath + "enemies.json";
        string abilityJsonPath = outputPath + "abilities.json";

        // Write json files
        File.WriteAllText(levelJsonPath, JsonUtility.ToJson(levelContainer, true));
        File.WriteAllText(itemJsonPath, JsonUtility.ToJson(itemContainer, true));
        File.WriteAllText(enemyJsonPath, JsonUtility.ToJson(enemyContainer, true));
        File.WriteAllText(abilityJsonPath, JsonUtility.ToJson(abilityContainer, true));
    }
}

// Wrapper classes for serialization

[Serializable]
class Levels
{
    public List<Level> levels;
}

[Serializable]
class Items
{
    public List<Item> items;
}

[Serializable]
class Enemies
{
    public List<Enemy> enemies;
}

[Serializable]
class Abilities
{
    public List<Ability> abilities;
}
