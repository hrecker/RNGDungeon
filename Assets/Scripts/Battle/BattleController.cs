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
    public GameObject exitToMenuButton;
    public GameObject itemIconPrefab;
    public GameObject itemDropPanel;

    public Text playerRollUI;
    public Text enemyRollUI;
    public Text previousPlayerRoll1UI;
    public Text previousEnemyRoll1UI;
    public Text previousPlayerRoll2UI;
    public Text previousEnemyRoll2UI;

    public RollGenerator playerRollGenerator;
    public RollGenerator enemyRollGenerator;
    public BattleStatus playerBattleStatus;
    public BattleStatus enemyBattleStatus;

    private List<Item> itemModsToAdd;
    private List<Modifier> currentRollBoundedMods;

    private void Start()
    {
        exitToMenuButton.SetActive(false);
        itemDropPanel.SetActive(false);
        itemModsToAdd = new List<Item>();
        currentRollBoundedMods = new List<Modifier>();
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
        foreach (Item itemMod in itemModsToAdd)
        {
            //TODO implement priority
            Modifier mod = itemMod.CreateItemModifier();
            PlayerStatus.Mods.RegisterModifier(mod, itemMod.modEffect.modPriority);
            currentRollBoundedMods.Add(mod);
        }
        itemModsToAdd.Clear();

        // Generate roll numeric values
        int playerInitial = playerRollGenerator.generateInitialRoll();
        int enemyInitial = enemyRollGenerator.generateInitialRoll();
        Tuple<int, int> rollValues = new Tuple<int, int>(playerInitial, enemyInitial);
        // Apply enemy mods first, then player mods to get final roll values
        // TODO enemy mods
        foreach (RollValueModifier mod in PlayerStatus.Mods.GetRollValueModifiers())
        {
            rollValues = mod.apply(rollValues.Item1, rollValues.Item2);
            DecrementAndDeregisterIfNecessary(mod);
        }

        // Generate roll results
        int playerDamage = Math.Max(0, rollValues.Item2 - rollValues.Item1);
        int enemyDamage = Math.Max(0, rollValues.Item1 - rollValues.Item2);
        RollResult rollResult = new RollResult 
        { PlayerDamage = playerDamage, EnemyDamage = enemyDamage };
        // Again apply enemy result mods forst, then player
        //TODO enemy mods
        foreach (RollResultModifier mod in PlayerStatus.Mods.GetRollResultModifiers())
        {
            rollResult = mod.apply(rollResult);
            DecrementAndDeregisterIfNecessary(mod);
        }

        // Apply roll results
        playerBattleStatus.ApplyResult(rollResult);
        enemyBattleStatus.ApplyResult(rollResult);

        CheckBattleComplete();

        UpdateRollUI(rollValues.Item1, rollValues.Item2, !completed);
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

    public static void DecrementAndDeregisterIfNecessary(Modifier mod)
    {
        if (mod.isRollBounded)
        {
            mod.numRollsRemaining--;
            if (mod.numRollsRemaining <= 0)
            {
                mod.DeregisterSelf();
            }
        }
    }

    private void UpdateRollUI(int playerRoll, int enemyRoll, bool fade)
    {
        // Update text values
        previousEnemyRoll2UI.text = previousEnemyRoll1UI.text;
        previousPlayerRoll2UI.text = previousPlayerRoll1UI.text;

        previousPlayerRoll1UI.text = playerRollUI.text;
        previousEnemyRoll1UI.text = enemyRollUI.text;

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
