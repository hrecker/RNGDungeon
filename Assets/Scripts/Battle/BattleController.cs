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
    public RollResultGenerator playerRollResultGenerator;
    public RollResultGenerator enemyRollResultGenerator;
    public BattleStatus playerBattleStatus;
    public BattleStatus enemyBattleStatus;

    private Dictionary<RollResultModifier, int> modsToAdd;

    private void Start()
    {
        exitToMenuButton.SetActive(false);
        itemDropPanel.SetActive(false);
        modsToAdd = new Dictionary<RollResultModifier, int>();
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
            playerRollResultGenerator == null || enemyRollResultGenerator == null ||
            playerBattleStatus == null || enemyBattleStatus == null)
        {
            Debug.LogWarning("Skipping roll - one or more rollgenerators, " +
                "rollresultgenerators, or battlestatuses are null");
            return;
        }

        // If there are modifiers to add, add before the roll starts
        foreach (RollResultModifier mod in modsToAdd.Keys)
        {
            if (mod.IsPlayer())
            {
                playerRollResultGenerator.AddModifier(mod, modsToAdd[mod]);
            }
            else
            {
                enemyRollResultGenerator.AddModifier(mod, modsToAdd[mod]);
            }
        }
        modsToAdd.Clear();

        // Generate roll numeric values
        int playerInitial = playerRollGenerator.generateInitialRoll();
        int enemyInitial = enemyRollGenerator.generateInitialRoll();
        Tuple<int, int> rollValues = new Tuple<int, int>(playerInitial, enemyInitial);
        // Apply enemy mods first, then player mods to get final roll values
        rollValues = enemyRollGenerator.applyPostRollModifiers(rollValues);
        rollValues = playerRollGenerator.applyPostRollModifiers(rollValues);

        // Generate roll results
        int playerDamage = Math.Max(0, rollValues.Item2 - rollValues.Item1);
        int enemyDamage = Math.Max(0, rollValues.Item1 - rollValues.Item2);
        RollResult rollResult = new RollResult 
        { PlayerDamage = playerDamage, EnemyDamage = enemyDamage };
        // Again apply enemy result mods forst, then player
        rollResult = enemyRollResultGenerator.applyModifiers(rollResult);
        rollResult = playerRollResultGenerator.applyModifiers(rollResult);

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

        PlayerStatus.Inventory[item]--;
        if (item.itemEffect.rollBoundedEffect != RollBoundedEffect.NONE)
        {
            switch (item.itemEffect.rollBoundedEffect)
            {
                case RollBoundedEffect.BLOCK:
                    modsToAdd.Add(new BlockingRollResultModifier(true), 
                        item.itemEffect.numRollsInEffect);
                    break;
                case RollBoundedEffect.RECOIL:
                    modsToAdd.Add(new RecoilRollResultModifer(true), 
                        item.itemEffect.numRollsInEffect);
                    break;
            }
        }
        playerBattleStatus.ApplyHealthChange(item.itemEffect.playerHealthChange);
        //TODO should roll buffs be bounded?
        playerRollGenerator.minRoll += item.itemEffect.playerMinRollChange;
        playerRollGenerator.maxRoll += item.itemEffect.playerMaxRollChange;
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
