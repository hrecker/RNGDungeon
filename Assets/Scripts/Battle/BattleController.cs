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
    public GameObject enemyHealthChangeUI;
    private Image playerHealthChangeBackground;
    private Image enemyHealthChangeBackground;
    private Text playerHealthChangeText;
    private Text enemyHealthChangeText;

    public RollGenerator playerRollGenerator;
    public RollGenerator enemyRollGenerator;
    public BattleStatus playerBattleStatus;
    public BattleStatus enemyBattleStatus;

    private List<Item> itemModsToAdd;
    private List<Modifier> currentRollBoundedMods;
    private Dictionary<string, Modifier> techMods;

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
            // If player wins, return to map. Otherwise just stop here.
            //TODO game end screen etc.
            if (playerBattleStatus.currentHealth > 0)
            {
                SceneManager.LoadScene("MapScene");
            }
        }
    }

    private void Roll()
    {
        if (playerRollGenerator == null || enemyRollGenerator == null ||
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
            PlayerStatus.Mods.RegisterModifier(techMods[selected.name],
                selected.modEffect.modPriority);
            currentRollBoundedMods.Add(techMods[selected.name]);
        }
        techUI.Roll();
        itemModsToAdd.Clear();

        // Generate roll numeric values
        int playerInitial = playerRollGenerator.generateInitialRoll();
        int enemyInitial = enemyRollGenerator.generateInitialRoll();
        Tuple<int, int> rollValues = new Tuple<int, int>(playerInitial, enemyInitial);
        // Apply enemy mods first, then player mods to get final roll values
        // TODO enemy mods
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
        //TODO enemy mods
        foreach (IRollResultModifier mod in PlayerStatus.Mods.GetRollResultModifiers())
        {
            rollResult = mod.apply(rollResult);
        }

        // Apply roll results
        playerBattleStatus.ApplyResult(rollResult);
        enemyBattleStatus.ApplyResult(rollResult);

        // Decrement roll-bounded mods
        PlayerStatus.Mods.DecrementAndDeregisterModsIfNecessary();

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
                resultText.text = "Victory";
                GenerateItemDrop();
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

        PlayerStatus.UseItem(item);
        if (item.modType != ModType.NONE)
        {
            itemModsToAdd.Add(item);
        }
        else
        {
            item.ApplyEffect();
        }
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
}
