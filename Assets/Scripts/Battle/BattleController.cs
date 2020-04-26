using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Battle.Enemies;
using Data;
using Modifiers;
using Levels;
using System.Linq;

namespace Battle
{
    public class BattleController : MonoBehaviour
    {
        public float rollInterval = 1.0f;
        public float victoryDisplayTime = 2.0f;
        private float timer = 0.0f;
        private bool completed;
        private bool fastforwardActive;
        private float baseRollInterval;

        public GameObject enemyGameObject;
        public GameObject resultTextPanel;
        public TechButtonController techUI;
        public GameObject exitToMenuButton;
        public GameObject itemIconPrefab;
        public GameObject itemDropPanel;
        private StatusUI statusUI;

        public Text rollCountText;
        public Text playerRollUI;
        public Text enemyRollUI;
        public GameObject playerRollDamageUI;
        public GameObject playerRollHealUI;
        private GameObject enemyRollDamageUI;
        private GameObject enemyRollHealUI;
        private Text playerRollDamageText;
        private Text playerRollHealText;
        private Text enemyRollDamageText;
        private Text enemyRollHealText;

        public RollGenerator playerRollGenerator;
        public Transform enemyParent;
        private EnemyBattleController enemyController;
        private RollGenerator enemyRollGenerator;
        public BattleHealthBar playerHealthBar;
        private BattleHealthBar enemyHealthBar;

        public RectTransform playerStatusMessagesParent;
        public RectTransform enemyStatusMessagesParent;
        public RectTransform playerModMessagesParent;
        public RectTransform enemyModMessagesParent;
        public GameObject statusMessagePrefab;
        public float statusMessageSpacing = 40.0f;

        private static List<string> playerStatusMessagesToShow;
        private static List<string> enemyStatusMessagesToShow;
        private static List<string> playerModMessagesToShow;
        private static List<string> enemyModMessagesToShow;
        private static int currentRoll;
        private bool firstUpdate;

        private void Awake()
        {
            statusUI = GetComponent<StatusUI>();
            baseRollInterval = rollInterval;
            // Add enemycontroller for the given enemy
            string enemyName = CurrentLevel.currentEnemyName;
            Enemy enemy = Data.Cache.GetEnemy(enemyName);
            EnemyStatus.Initialize(enemy);
            Type controllerType = enemy.GetEnemyControllerType();
            enemyGameObject.AddComponent(controllerType);
            enemyController = enemyGameObject.GetComponent<EnemyBattleController>();
            enemyRollGenerator = enemyGameObject.GetComponent<RollGenerator>();
            enemyHealthBar = enemyGameObject.GetComponent<BattleHealthBar>();
            enemyRollDamageUI = enemyGameObject.gameObject.transform.Find("RollDamage").gameObject;
            enemyRollHealUI = enemyGameObject.gameObject.transform.Find("RollHeal").gameObject;
            playerModMessagesToShow = new List<string>();
            enemyModMessagesToShow = new List<string>();
            enemyHealthBar.status = EnemyStatus.Status;
            playerHealthBar.status = PlayerStatus.Status;
        }

        private void Start()
        {
            firstUpdate = true;
            currentRoll = 1;
            UpdateRollCountUI();
            exitToMenuButton.SetActive(false);
            itemDropPanel.SetActive(false);
            resultTextPanel.SetActive(false);
            playerRollDamageText = playerRollDamageUI.GetComponentInChildren<Text>();
            playerRollHealText = playerRollHealUI.GetComponentInChildren<Text>();
            enemyRollDamageText = enemyRollDamageUI.GetComponentInChildren<Text>();
            enemyRollHealText = enemyRollHealUI.GetComponentInChildren<Text>();
            playerStatusMessagesToShow = new List<string>();
            enemyStatusMessagesToShow = new List<string>();
            playerRollDamageUI.SetActive(false);
            playerRollHealUI.SetActive(false);
            enemyRollDamageUI.SetActive(false);
            enemyRollHealUI.SetActive(false);
            CheckBattleComplete();
        }

        void Update()
        {
            if (firstUpdate)
            {
                statusUI.UpdateStatusIcons();
                firstUpdate = false;
            }

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
                if (PlayerStatus.Status.Health > 0 && CurrentLevel.currentEnemyName != "Boss")
                {
                    SceneManager.LoadScene("MapScene");
                }
            }
        }

        public void ToggleFastforward()
        {
            float intervalFractionCompleted = timer / rollInterval;
            if (fastforwardActive)
            {
                rollInterval = baseRollInterval;
            }
            else
            {
                rollInterval = baseRollInterval / 2;
            }
            timer = intervalFractionCompleted * rollInterval;
            fastforwardActive = !fastforwardActive;
        }

        private void Roll()
        {
            // If there are modifiers to add, add before the roll starts
            // Add mods for selected techs
            Tech selectedTech = null;
            if (techUI.GetSelectedTech() != null)
            {
                selectedTech = techUI.GetSelectedTech();
                // Add UI messages if necessary
                playerStatusMessagesToShow.Add(selectedTech.playerStatusMessage);
                enemyStatusMessagesToShow.Add(selectedTech.enemyStatusMessage);
                PlayerStatus.Status.Mods.RegisterModifier(selectedTech.CreateTechModifier());
            }
            // Add any other mods that should be active before the roll
            foreach (Modifier mod in PlayerStatus.Status.NextRollMods)
            {
                PlayerStatus.Status.Mods.RegisterModifier(mod);
            }
            PlayerStatus.Status.NextRollMods.Clear();
            foreach (Modifier mod in EnemyStatus.Status.NextRollMods)
            {
                EnemyStatus.Status.Mods.RegisterModifier(mod);
            }
            EnemyStatus.Status.NextRollMods.Clear();

            // Generate roll numeric values
            int playerInitial = playerRollGenerator.GenerateInitialRoll();
            int enemyInitial = enemyRollGenerator.GenerateInitialRoll();
            Tuple<int, int> rollValues = new Tuple<int, int>(playerInitial, enemyInitial);
            // Apply enemy and player mods to get final roll values
            IEnumerable<IRollValueModifier> rvMods = 
                PlayerStatus.Status.Mods.GetRollValueModifiers().Union(
                EnemyStatus.Status.Mods.GetRollValueModifiers()).
                OrderBy(m => ((Modifier)m).priority);
            foreach (IRollValueModifier mod in rvMods)
            {
                rollValues = mod.ApplyRollValueMod(rollValues.Item1, rollValues.Item2);
            }

            // Generate roll results
            int playerDamage = Math.Max(0, rollValues.Item2 - rollValues.Item1);
            int enemyDamage = Math.Max(0, rollValues.Item1 - rollValues.Item2);
            RollResult rollResult = new RollResult
            { 
                PlayerRollDamage = playerDamage, 
                EnemyRollDamage = enemyDamage,
                PlayerTech = selectedTech
            };
            // Again apply enemy and player mods
            IEnumerable<IRollResultModifier> rrMods =
                PlayerStatus.Status.Mods.GetRollResultModifiers().Union(
                EnemyStatus.Status.Mods.GetRollResultModifiers()).
                OrderBy(m => ((Modifier)m).priority);
            foreach (IRollResultModifier mod in rrMods)
            {
                rollResult = mod.ApplyRollResultMod(rollResult);
            }

            // Apply roll results
            playerHealthBar.ApplyResult(rollResult);
            enemyHealthBar.ApplyResult(rollResult);

            // Apply enemy and player post damage effects
            IEnumerable<IPostDamageModifier> pdMods =
                PlayerStatus.Status.Mods.GetPostDamageModifiers().Union(
                EnemyStatus.Status.Mods.GetPostDamageModifiers()).
                OrderBy(m => ((Modifier)m).priority);
            foreach (IPostDamageModifier mod in pdMods)
            {
                mod.ApplyPostDamageMod(rollResult);
            }

            // Decrement roll-bounded mods
            PlayerStatus.Status.Mods.DecrementAndDeregisterModsIfNecessary();
            EnemyStatus.Status.Mods.DecrementAndDeregisterModsIfNecessary();
            currentRoll++;
            
            statusUI.UpdateStatusIcons();

            CheckBattleComplete();
            foreach (Tech tech in PlayerStatus.EnabledTechs)
            {
                tech.DecrementCooldown();
            }
            techUI.Roll();

            if (!completed)
            {
                UpdateRollCountUI();
            }
            UpdateRollUI(rollValues.Item1, rollValues.Item2, !completed);
            UpdateHealthUI(rollResult, !completed);

            // Battle status messages
            CreateStatusMessages();
            CreateModMessages();
            playerStatusMessagesToShow.Clear();
            enemyStatusMessagesToShow.Clear();
            playerModMessagesToShow.Clear();
            enemyModMessagesToShow.Clear();
        }

        private void CheckBattleComplete()
        {
            // Disable when the battle is over, and display result
            if (PlayerStatus.Status.Health <= 0 || EnemyStatus.Status.Health <= 0)
            {
                // End any roll-bounded modifiers
                PlayerStatus.Status.Mods.DeregisterAllRollBoundedMods();
                PlayerStatus.Status.NextRollMods.Clear();

                // Reset roll count
                currentRoll = 0;

                if (PlayerStatus.Status.Health > 0)
                {
                    if (CurrentLevel.currentEnemyName == "Boss")
                    {
                        ShowResult("Dungeon Defeated!", true);
                    }
                    else
                    {
                        ShowResult("Victory", false);
                        GenerateItemDrop();
                        // Apply any post battle mods when player defeats enemy
                        IEnumerable<IPostBattleModifier> pbMods =
                            PlayerStatus.Status.Mods.GetPostBattleModifiers().Union(
                            EnemyStatus.Status.Mods.GetPostBattleModifiers()).
                            OrderBy(m => ((Modifier)m).priority);
                        foreach (IPostBattleModifier mod in pbMods)
                        {
                            mod.ApplyPostBattleMod();
                        }
                    }
                }
                else
                {
                    ShowResult("Defeat", true);
                }
                completed = true;
            }
        }

        private void ShowResult(string resultText, bool showExitButton)
        {
            resultTextPanel.SetActive(true);
            resultTextPanel.GetComponentInChildren<Text>().text = resultText;
            exitToMenuButton.SetActive(showExitButton);
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

        private void UpdateHealthUI(RollResult rollResult, bool fade)
        {
            UpdateHealthUI(rollResult.PlayerDamage, playerRollDamageUI,
                playerRollDamageText, fade);
            UpdateHealthUI(rollResult.PlayerHeal, playerRollHealUI,
                playerRollHealText, fade);
            UpdateHealthUI(rollResult.EnemyDamage, enemyRollDamageUI,
                enemyRollDamageText, fade);
            UpdateHealthUI(rollResult.EnemyHeal, enemyRollHealUI,
                enemyRollHealText, fade);
        }

        private void UpdateHealthUI(int healthDiff, GameObject uiParent, Text text, bool fade)
        {
            if (healthDiff == 0)
            {
                uiParent.SetActive(false);
            }
            else
            {
                uiParent.GetComponent<Image>().CrossFadeAlpha(1, 0, false);
                text.CrossFadeAlpha(1, 0, false);
                uiParent.SetActive(true);
                text.text = healthDiff.ToString();
                if (fade)
                {
                    uiParent.GetComponent<Image>().CrossFadeAlpha(0, rollInterval, false);
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
            if (PlayerStatus.UseItem(item, true))
            {
                // Add status messages if there are any
                CreateMessages(new List<string>() { item.playerStatusMessage },
                    new List<string>() { item.enemyStatusMessage },
                    playerStatusMessagesParent, enemyStatusMessagesParent);
                // Update status icons in case status has changed
                statusUI.UpdateStatusIcons();
                return true;
            }
            return false;
        }

        public void GenerateItemDrop()
        {
            Item itemDrop = CurrentLevel.GetEnemyItemDrop();
            if (itemDrop != null)
            {
                if (itemDrop.name == "Key")
                {
                    PlayerStatus.KeyCount++;
                }
                else
                {
                    PlayerStatus.AddItem(itemDrop);
                }
                itemDropPanel.SetActive(true);
                GameObject newIcon = BattleItemUI.InstantiateItemIcon(itemDrop,
                    itemIconPrefab, itemDropPanel.transform, false);
                newIcon.GetComponent<ItemIcon>().ItemCount = 1;
            }
        }

        public static void AddStatusMessage(BattleActor actor, string message)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    playerStatusMessagesToShow.Add(message);
                    break;
                case BattleActor.ENEMY:
                    enemyStatusMessagesToShow.Add(message);
                    break;
            }
        }

        public static void AddModMessage(BattleActor actor, string message)
        {
            switch (actor)
            {
                case BattleActor.PLAYER:
                    playerModMessagesToShow.Add(message);
                    break;
                case BattleActor.ENEMY:
                    enemyModMessagesToShow.Add(message);
                    break;
            }
        }

        private void CreateModMessages()
        {
            CreateMessages(playerModMessagesToShow, enemyModMessagesToShow,
                playerModMessagesParent, enemyModMessagesParent);
        }

        private void CreateStatusMessages()
        {
            CreateMessages(playerStatusMessagesToShow, enemyStatusMessagesToShow,
                playerStatusMessagesParent, enemyStatusMessagesParent);
        }

        private void CreateMessages(List<string> playerMessages, List<string> enemyMessages, 
            RectTransform playerMessageParent, RectTransform enemyMessageParent)
        {
            for (int i = 0; i < playerMessages.Count; i++)
            {
                CreateStatusMessage(playerMessages[i], playerMessageParent, i);
            }
            for (int i = 0; i < enemyMessages.Count; i++)
            {
                CreateStatusMessage(enemyMessages[i], enemyMessageParent, i);
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

        public static int GetCurrentRoll()
        {
            return currentRoll;
        }

        private void UpdateRollCountUI()
        {
            rollCountText.text = "Roll " + currentRoll;
        }
    }
}
