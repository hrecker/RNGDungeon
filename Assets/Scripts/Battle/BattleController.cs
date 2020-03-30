using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public float rollInterval = 1.0f;
    public float victoryDisplayTime = 2.0f;
    private float timer = 0.0f;
    private bool completed;

    public Text resultText;
    public TechButtonController techUI;
    public GameObject exitToMenuButton;
    public GameObject itemIconPrefab;
    public GameObject itemDropPanel;

    public Text playerRollUI;
    public Text enemyRollUI;
    public Color healthLossColor;
    public Color healthGainColor;
    public GameObject playerHealthChangeUI;
    private GameObject enemyHealthChangeUI;
    private Image playerHealthChangeBackground;
    private Image enemyHealthChangeBackground;
    private Text playerHealthChangeText;
    private Text enemyHealthChangeText;

    public RollGenerator playerRollGenerator;
    public Transform enemyParent;
    private EnemyBattleController enemyController;
    public BattleStatus playerBattleStatus;
    private BattleStatus enemyBattleStatus;

    public RectTransform playerStatusMessagesParent;
    public RectTransform enemyStatusMessagesParent;
    public GameObject statusMessagePrefab;
    public float statusMessageSpacing = 40.0f;

    private List<Item> itemModsToAdd;
    private List<Modifier> currentRollBoundedMods;
    private Dictionary<string, Modifier> techMods;
    private List<string> playerStatusMessagesToShow;
    private List<string> enemyStatusMessagesToShow;

    private const string enemyPrefabResourcePath = @"Enemies/battleprefabs/";

    private void Awake()
    {
        // Instantiate enemy prefab
        string enemyName = CurrentLevel.currentEnemyName;
        GameObject newEnemy = (GameObject) Instantiate(
            Resources.Load(enemyPrefabResourcePath + enemyName), enemyParent);
        enemyController = newEnemy.GetComponent<EnemyBattleController>();
        enemyBattleStatus = newEnemy.GetComponent<EnemyBattleStatus>();
        enemyHealthChangeUI = newEnemy.gameObject.transform.Find("RollDamage").gameObject;
        newEnemy.transform.SetSiblingIndex(0);
    }

    private void Start()
    {
        exitToMenuButton.SetActive(false);
        itemDropPanel.SetActive(false);
        itemModsToAdd = new List<Item>();
        currentRollBoundedMods = new List<Modifier>();
        techMods = new Dictionary<string, Modifier>();
        playerHealthChangeBackground = playerHealthChangeUI.GetComponent<Image>();
        playerHealthChangeText = playerHealthChangeUI.GetComponentInChildren<Text>();
        enemyHealthChangeBackground = enemyHealthChangeUI.GetComponent<Image>();
        enemyHealthChangeText = enemyHealthChangeUI.GetComponentInChildren<Text>();
        playerStatusMessagesToShow = new List<string>();
        enemyStatusMessagesToShow = new List<string>();
        playerHealthChangeUI.SetActive(false);
        enemyHealthChangeUI.SetActive(false);
        CheckBattleComplete();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!completed && timer >= rollInterval)
        {
            Roll();
            timer = timer - rollInterval;
        }
        else if (completed && timer >= victoryDisplayTime)
        {
            enabled = false;
            // If player wins and it's not the boss fight, return to map. Otherwise just stop here.
            if (playerBattleStatus.currentHealth > 0 && CurrentLevel.currentEnemyName != "Boss")
            {
                SceneManager.LoadScene("MapScene");
            }
        }
    }

    private void Roll()
    {
        if (playerRollGenerator == null || enemyController == null ||
            playerBattleStatus == null || enemyBattleStatus == null)
        {
            Debug.LogWarning("Skipping roll - one or more rollgenerators " +
                "or battlestatuses are null");
            return;
        }

        // If there are modifiers to add, add before the roll starts
        // First, add mods for and used items
        foreach (Item itemMod in itemModsToAdd)
        {
            Modifier mod = itemMod.CreateItemModifier();
            PlayerStatus.Mods.RegisterModifier(mod, itemMod.modEffect.modPriority);
            currentRollBoundedMods.Add(mod);
        }
        // Add mods for selected techs
        if (techUI.GetSelectedTech() != null)
        {
            Tech selected = techUI.GetSelectedTech();
            if (!techMods.ContainsKey(selected.name))
            {
                techMods.Add(selected.name, selected.CreateTechModifier());
            }
            // Add UI messages if necessary
            playerStatusMessagesToShow.Add(selected.playerStatusMessage);
            enemyStatusMessagesToShow.Add(selected.enemyStatusMessage);
            PlayerStatus.Mods.RegisterModifier(techMods[selected.name],
                selected.modEffect.modPriority);
            currentRollBoundedMods.Add(techMods[selected.name]);
        }
        techUI.Roll();
        itemModsToAdd.Clear();

        // Generate roll numeric values
        int playerInitial = playerRollGenerator.GenerateInitialRoll();
        int enemyInitial = enemyController.GenerateInitialRoll();
        Tuple<int, int> rollValues = new Tuple<int, int>(playerInitial, enemyInitial);
        // Apply enemy mods first, then player mods to get final roll values
        rollValues = enemyController.ApplyRollValueMods(playerInitial, enemyInitial);
        foreach (IRollValueModifier mod in PlayerStatus.Mods.GetRollValueModifiers())
        {
            rollValues = mod.apply(rollValues.Item1, rollValues.Item2);
        }

        // Generate roll results
        int playerDamage = Math.Max(0, rollValues.Item2 - rollValues.Item1);
        int enemyDamage = Math.Max(0, rollValues.Item1 - rollValues.Item2);
        RollResult rollResult = new RollResult 
        { PlayerDamage = playerDamage, EnemyDamage = enemyDamage };
        // Again apply enemy result mods forst, then player
        rollResult = enemyController.ApplyRollResultMods(rollResult);
        foreach (IRollResultModifier mod in PlayerStatus.Mods.GetRollResultModifiers())
        {
            rollResult = mod.apply(rollResult);
        }

        // Apply roll results
        playerBattleStatus.ApplyResult(rollResult);
        enemyBattleStatus.ApplyResult(rollResult);

        // Post damage effects
        enemyController.ApplyPostDamageEffects(rollResult);

        // Decrement roll-bounded mods
        PlayerStatus.Mods.DecrementAndDeregisterModsIfNecessary();

        // Battle status messages
        CreateStatusMessages(playerStatusMessagesToShow, enemyStatusMessagesToShow);
        playerStatusMessagesToShow.Clear();
        enemyStatusMessagesToShow.Clear();

        CheckBattleComplete();

        UpdateRollUI(rollValues.Item1, rollValues.Item2, !completed);
        UpdateHealthUI(-rollResult.PlayerDamage, -rollResult.EnemyDamage, !completed);
    }

    private void CheckBattleComplete()
    {
        // Disable when the battle is over, and display result
        if (playerBattleStatus.currentHealth <= 0 || enemyBattleStatus.currentHealth <= 0)
        {
            // End any roll-bounded modifiers
            foreach (Modifier mod in currentRollBoundedMods)
            {
                mod.DeregisterSelf();
            }

            if (playerBattleStatus.currentHealth > 0)
            {
                if (CurrentLevel.currentEnemyName == "Boss")
                {
                    resultText.text = "Dungeon Defeated!";
                    exitToMenuButton.SetActive(true);
                }
                else
                {
                    resultText.text = "Victory";
                    GenerateItemDrop();
                }
            }
            else
            {
                resultText.text = "Defeat";
                exitToMenuButton.SetActive(true);
            }
            completed = true;
        }
    }

    private void UpdateRollUI(int playerRoll, int enemyRoll, bool fade)
    {
        // Reset the alpha for the texts
        playerRollUI.CrossFadeAlpha(1, 0, false);
        enemyRollUI.CrossFadeAlpha(1, 0, false);
        playerRollUI.text = playerRoll.ToString();
        enemyRollUI.text = enemyRoll.ToString();

        if (fade)
        {
            // Fade out over the roll interval
            playerRollUI.CrossFadeAlpha(0, rollInterval, false);
            enemyRollUI.CrossFadeAlpha(0, rollInterval, false);
        }
    }

    private void UpdateHealthUI(int playerHealthDiff, int enemyHealthDiff, bool fade)
    {
        UpdateHealthUI(playerHealthDiff, playerHealthChangeUI,
            playerHealthChangeBackground, playerHealthChangeText, fade);
        UpdateHealthUI(enemyHealthDiff, enemyHealthChangeUI, 
            enemyHealthChangeBackground, enemyHealthChangeText, fade);
    }

    private void UpdateHealthUI(int healthDiff, GameObject uiParent, Image background, Text text, bool fade)
    {
        if (healthDiff == 0)
        {
            uiParent.SetActive(false);
        }
        else
        {
            background.CrossFadeAlpha(1, 0, false);
            text.CrossFadeAlpha(1, 0, false);
            uiParent.SetActive(true);
            background.color =
                healthDiff > 0 ? healthGainColor : healthLossColor;
            text.text = Math.Abs(healthDiff).ToString();
            if (fade)
            {
                background.CrossFadeAlpha(0, rollInterval, false);
                text.CrossFadeAlpha(0, rollInterval, false);
            }
        }
    }

    public bool ApplyPlayerItem(Item item)
    {
        if (completed)
        {
            return false;
        }

        // Update PlayerStatus
        PlayerStatus.UseItem(item);
        // Add mod or apply effect directly
        if (item.modType != ModType.NONE)
        {
            itemModsToAdd.Add(item);
        }
        else
        {
            item.ApplyEffect();
        }
        // Add status messages if there are any
        CreateStatusMessages(new List<string>() { item.playerStatusMessage },
            new List<string>() { item.enemyStatusMessage });
        return true;
    }

    public void GenerateItemDrop()
    {
        string itemDrop = CurrentLevel.GetEnemyItemDrop();
        if (itemDrop != null)
        {
            Item item = Cache.GetItem(itemDrop);
            PlayerStatus.AddItem(item);
            itemDropPanel.SetActive(true);
            GameObject newIcon = BattleItemUI.InstantiateItemIcon(item, 
                itemIconPrefab, itemDropPanel.transform, false);
            newIcon.GetComponent<ItemIcon>().ItemCount = 1;
        }
    }

    public void AddRollBoundedMod(Modifier mod, int priority)
    {
        AddRollBoundedMod(mod, priority, null, null);
    }

    // Used when something else (an enemy) needs to add a mod that ends when the battle ends
    public void AddRollBoundedMod(Modifier mod, int priority, string playerUiMessage, string enemyUiMessage)
    {
        PlayerStatus.Mods.RegisterModifier(mod, priority);
        currentRollBoundedMods.Add(mod);
        playerStatusMessagesToShow.Add(playerUiMessage);
        enemyStatusMessagesToShow.Add(enemyUiMessage);
    }

    private void CreateStatusMessages(List<string> playerMessages, List<string> enemyMessages)
    {
        if (playerMessages != null)
        {
            for (int i = 0; i < playerMessages.Count; i++)
            {
                CreateStatusMessage(playerMessages[i], playerStatusMessagesParent, i);
            }
        }
        if (enemyMessages != null)
        {
            for (int i = 0; i < enemyMessages.Count; i++)
            {
                CreateStatusMessage(enemyMessages[i], enemyStatusMessagesParent, i);
            }
        }
    }

    private void CreateStatusMessage(string message, RectTransform parent, int index)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        GameObject newMessage = Instantiate(statusMessagePrefab, parent);
        Text messageText = newMessage.GetComponentInChildren<Text>();
        messageText.text = message;
        if (index > 0)
        {
            newMessage.GetComponent<RectTransform>().anchoredPosition +=
                (statusMessageSpacing * index * Vector2.down);
        }
    }
}
