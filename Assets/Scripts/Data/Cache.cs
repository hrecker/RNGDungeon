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
    private static Dictionary<string, Tech> techs;
    private static Dictionary<string, Sprite> itemIcons;
    private static Dictionary<string, Sprite> abilityIcons;
    private static Dictionary<string, Sprite> enemyIcons;
    private static Dictionary<string, Sprite> techIcons;

    private static string levelsJsonPath = @"Levels/levels";
    private static string itemsJsonPath = @"Items/items";
    private static string enemiesJsonPath = @"Enemies/enemies";
    private static string abilitiesJsonPath = @"Abilities/abilities";
    private static string techsJsonPath = @"Techs/techs";

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
        techs = new Dictionary<string, Tech>();
        itemIcons = new Dictionary<string, Sprite>();
        enemyIcons = new Dictionary<string, Sprite>();
        abilityIcons = new Dictionary<string, Sprite>();
        techIcons = new Dictionary<string, Sprite>();

        // Load JSON from resouce files
        TextAsset levelsFile = Resources.Load<TextAsset>(levelsJsonPath);
        TextAsset itemsFile = Resources.Load<TextAsset>(itemsJsonPath);
        TextAsset enemiesFile = Resources.Load<TextAsset>(enemiesJsonPath);
        TextAsset abilitiesFile = Resources.Load<TextAsset>(abilitiesJsonPath);
        TextAsset techsFile = Resources.Load<TextAsset>(techsJsonPath);

        Levels levelsContainer = JsonUtility.FromJson<Levels>(levelsFile.text);
        Items itemsContainer = JsonUtility.FromJson<Items>(itemsFile.text);
        Enemies enemiesContainer = JsonUtility.FromJson<Enemies>(enemiesFile.text);
        Abilities abilitiesContainer = JsonUtility.FromJson<Abilities>(abilitiesFile.text);
        Techs techsContainer = JsonUtility.FromJson<Techs>(techsFile.text);

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
            enemyIcons.Add(enemy.name, Resources.Load<Sprite>(@"Enemies/sprites/" + enemy.name));
        }
        foreach (Ability ability in abilitiesContainer.abilities)
        {
            abilities.Add(ability.name, ability);
            abilityIcons.Add(ability.name, Resources.Load<Sprite>(@"Abilities/sprites/" + ability.name));
        }
        foreach (Tech tech in techsContainer.techs)
        {
            techs.Add(tech.name, tech);
            techIcons.Add(tech.name, Resources.Load<Sprite>(@"Techs/sprites/" + tech.name));
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
        if (name == null || !items.ContainsKey(name))
        {
            return null;
        }
        return items[name];
    }

    public static Item GetRandomItem()
    {
        return items.ElementAt(UnityEngine.Random.Range(0, items.Count)).Value;
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

    public static Sprite GetEnemyIcon(string name)
    {
        Load();
        return enemyIcons[name];
    }

    public static Ability GetAbility(string name)
    {
        Load();
        return abilities[name];
    }

    public static Sprite GetAbilityIcon(string name)
    {
        Load();
        return abilityIcons[name];
    }

    public static List<Ability> GetRandomAbilities(int numAbilities, List<Ability> disallowed)
    {
        List<Ability> chosenAbilities = new List<Ability>();
        List<Ability> allAbilities = new List<Ability>(abilities.Values);
        // Don't select abilities from the disallowed list
        foreach (Ability disallow in disallowed)
        {
            allAbilities.Remove(disallow);
        }
        for (int i = 0; i < numAbilities; i++)
        {
            Ability chosen = allAbilities[UnityEngine.Random.Range(0, allAbilities.Count)];
            // If there are enough abilities, prevent duplicates
            if (allAbilities.Count > 1)
            {
                allAbilities.Remove(chosen);
            }
            chosenAbilities.Add(chosen);
        }

        return chosenAbilities;
    }

    public static Tech GetTect(string name)
    {
        Load();
        return techs[name];
    }

    public static Sprite GetTechIcon(string name)
    {
        Load();
        return techIcons[name];
    }


    // Only to be used while developing - can't serialize to the resources
    // folder in the built game
    public static void SerializeExample()
    {
        Levels levelContainer = new Levels() { levels = new List<Level>() };
        Items itemContainer = new Items() { items = new List<Item>() };
        Enemies enemyContainer = new Enemies() { enemies = new List<Enemy>() };
        Abilities abilityContainer = new Abilities() { abilities = new List<Ability>() };
        Techs techContainer = new Techs() { techs = new List<Tech>() };

        // Sample levels
        levelContainer.levels.Add(new Level()
        {
            floor = 1,
            roomHeight = 6,
            roomWidth = 6,
            numRooms = 8,
            encounterRate = 0.1f,
            enemies = new string[] { "Bat", "Slime" },
            enemyEncounterRates = new float[] { 0.5f, 0.5f }
        });
        levelContainer.levels.Add(new Level()
        {
            floor = 2,
            roomHeight = 8,
            roomWidth = 7,
            numRooms = 11,
            encounterRate = 0.15f,
            enemies = new string[] { "Bat", "Slime" },
            enemyEncounterRates = new float[] { 0.7f, 0.3f }
        });
        // Sample items
        itemContainer.items.Add(new Item() 
        { 
            name = "HealthPotion",
            playerHealthChange = 3,
            itemType = ItemType.USABLE_ANYTIME,
        });
        itemContainer.items.Add( new Item() 
        { 
            name = "BlockingPotion",
            modType = ModType.BLOCK,
            numRollsInEffect = 3,
        });
        itemContainer.items.Add(new Item()
        {
            name = "Shortsword",
            modType = ModType.WEAPON,
            modEffect = new ModEffect()
            {
                modPriority = 0,
                playerMinRollChange = 1,
                playerMaxRollChange = 1,
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
        abilityContainer.abilities.Add(new Ability() 
        { 
            name = "Stalwart",
            modType = ModType.STALWART,
            modEffect = new ModEffect()
            {
                baseModTriggerChance = 0.2f
            }
        });
        abilityContainer.abilities.Add(new Ability() 
        { 
            name = "HighRoller",
            modType = ModType.HIGHROLLER,
            modEffect = new ModEffect()
            {
                baseModTriggerChance = 0.1f
            }
        });
        // Sample techs
        techContainer.techs.Add(new Tech()
        {
            name = "HeavySwing",
            tooltip = "Increases rolls for one roll, followed by a two roll debuff",
            cooldownRolls = 6,
            numRollsInEffect = 3,
            modType = ModType.HEAVYSWING,
        });
        techContainer.techs.Add(new Tech()
        {
            name = "Rage",
            tooltip = "Increases rolls when on low health",
            cooldownRolls = 5,
            modType = ModType.RAGE,
        });
        techContainer.techs.Add(new Tech()
        {
            name = "Bulwark",
            tooltip = "Raises min roll, but does not deal damage",
            cooldownRolls = 8,
            modType = ModType.BULWARK,
            modEffect = new ModEffect()
            {
                playerMinRollChange = 3
            }
        });

        string outputPath = @"C:\Users\henry\SampleSerializedJson\";
        string levelJsonPath = outputPath + "levels.json";
        string itemJsonPath = outputPath + "items.json";
        string enemyJsonPath = outputPath + "enemies.json";
        string abilityJsonPath = outputPath + "abilities.json";
        string techJsonPath = outputPath + "techs.json";

        // Write json files
        File.WriteAllText(levelJsonPath, JsonUtility.ToJson(levelContainer, true));
        File.WriteAllText(itemJsonPath, JsonUtility.ToJson(itemContainer, true));
        File.WriteAllText(enemyJsonPath, JsonUtility.ToJson(enemyContainer, true));
        File.WriteAllText(abilityJsonPath, JsonUtility.ToJson(abilityContainer, true));
        File.WriteAllText(techJsonPath, JsonUtility.ToJson(techContainer, true));
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

[Serializable]
class Techs
{
    public List<Tech> techs;
}
