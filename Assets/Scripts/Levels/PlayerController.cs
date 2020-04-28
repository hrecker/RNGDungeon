using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UI;
using Data;
using Battle;

namespace Levels
{
    public class PlayerController : MonoBehaviour
    {
        public float moveDelay = 0.15f;
        public InventoryInput inventoryInput;
        public GameObject pickupPanel;
        public GameObject itemIconPrefab;
        public AbilityAndTechSelectionUI abilityAndTechSelection;
        public Text keyCountText;
        public HealthBar playerHealthBar;
        public Text currentFloorText;
        private float timer;
        private bool moving;
        private bool selectingAbility;
        private static List<Ability> availableAbilities;
        private static List<Tech> availableTechs;

        private PauseMenu pauseMenu;

        private Vector2 startPosition;
        private Vector2 targetPosition;

        private Dictionary<Rarity, float> itemPickupRarityChances;
        private Dictionary<Rarity, float> chestRarityChances;

        public static Item randomlyGivenItem;

        // This is just for testing individually the map scene
        private void Awake()
        {
            if (!GameInitialization.HasInitialized())
            {
                GameInitialization.StartNewGame();
            }
        }

        void Start()
        {
            pauseMenu = FindObjectOfType<PauseMenu>();
            pickupPanel.SetActive(false);
            if (!CurrentLevel.HasSelectedLevelAbility())
            {
                ShowAbilitySelectionUI();
            }
            else
            {
                abilityAndTechSelection.gameObject.SetActive(false);
            }
            transform.position = PlayerStatus.MapPosition;
            UpdateKeyCount();
            UpdateCurrentFloor();
            playerHealthBar.UpdateHealth(PlayerStatus.Status.Health, PlayerStatus.Status.MaxHealth);

            itemPickupRarityChances = new Dictionary<Rarity, float>()
            {
                { Rarity.COMMON, 0.35f },
                { Rarity.UNCOMMON, 0.5f },
                { Rarity.RARE, 0.15f }
            };
            chestRarityChances = new Dictionary<Rarity, float>()
            {
                { Rarity.UNCOMMON, 0.2f },
                { Rarity.RARE, 0.8f }
            };
        }

        void Update()
        {
            if (pauseMenu.IsPaused() || selectingAbility)
            {
                return;
            }

            // If a random item has been given to the player, display it
            if (randomlyGivenItem != null)
            {
                HandleItemPickup(randomlyGivenItem);
                randomlyGivenItem = null;
            }

            if (moving)
            {
                timer += Time.deltaTime;
                Move();
            }
            else
            {
                CheckInput();
            }
        }

        private void CheckInput()
        {
            int vertical = 0;
            if (Input.GetKey(KeyCode.W))
            {
                vertical = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                vertical = -1;
            }

            int horizontal = 0;
            if (vertical == 0 && Input.GetKey(KeyCode.A))
            {
                horizontal = -1;
            }
            else if (vertical == 0 && Input.GetKey(KeyCode.D))
            {
                horizontal = 1;
            }

            if (horizontal != vertical && CurrentLevel.CheckMovement(
                ((Vector2)transform.position) + new Vector2(horizontal, vertical)))
            {
                startPosition = transform.position;
                targetPosition = startPosition + new Vector2(horizontal, vertical);
                moving = true;
                timer = 0.0f;
                // Inventory can't be opened while moving
                inventoryInput.SetInventoryEnabled(false);
            }
        }

        private void Move()
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / moveDelay);
            if (timer >= moveDelay)
            {
                moving = false;
                PlayerStatus.MapPosition = targetPosition;
                // Check for effects after the on-screen move is done
                MoveResult result = CurrentLevel.Move(startPosition, targetPosition);
                switch (result)
                {
                    case MoveResult.BATTLE:
                        SceneManager.LoadScene("BattleScene");
                        break;
                    case MoveResult.STAIRSDOWN:
                        // Update player position to new level map position
                        PlayerStatus.MapPosition = CurrentLevel.GetPlayerStartingPosition();
                        transform.position = PlayerStatus.MapPosition;

                        ShowAbilitySelectionUI();
                        UpdateKeyCount();
                        UpdateCurrentFloor();
                        break;
                    case MoveResult.CHESTOPEN:
                        UpdateKeyCount();
                        PickupRandomItem(chestRarityChances);
                        inventoryInput.SetInventoryEnabled(true);
                        CheckInput();
                        break;
                    case MoveResult.ITEMPICKUP:
                        PickupRandomItem(itemPickupRarityChances);
                        inventoryInput.SetInventoryEnabled(true);
                        CheckInput();
                        break;
                    default:
                        // Check input as soon as move is finished so there's no jittering
                        inventoryInput.SetInventoryEnabled(true);
                        CheckInput();
                        break;
                }
            }
        }

        private void PickupRandomItem(Dictionary<Rarity, float> rarityChances)
        {
            Item pickedUp = Data.Cache.GetRandomItem(rarityChances);
            HandleItemPickup(pickedUp);
        }

        private void HandleItemPickup(Item pickedUp)
        {
            PlayerStatus.AddItem(pickedUp);
            // Update UI
            pickupPanel.SetActive(true);
            ItemIcon currentPickupIcon = pickupPanel.GetComponentInChildren<ItemIcon>();
            if (currentPickupIcon != null)
            {
                Destroy(currentPickupIcon.gameObject);
            }
            GameObject newIcon = BattleItemUI.InstantiateItemIcon(pickedUp,
                itemIconPrefab, pickupPanel.transform, false);
            newIcon.GetComponent<ItemIcon>().ItemCount = 1;
            pickupPanel.GetComponent<SelfDeactivate>().ResetTimer();
        }

        public void ShowAbilitySelectionUI()
        {
            // Allow viewing inventory during ability selection
            inventoryInput.SetInventoryEnabled(true);
            abilityAndTechSelection.gameObject.SetActive(true);
            if (availableAbilities == null || availableAbilities.Count == 0)
            {
                availableAbilities = Data.Cache.GetRandomAbilities(3, PlayerStatus.GetAbilities());
            }
            if (availableTechs == null || availableTechs.Count == 0)
            {
                availableTechs = Data.Cache.GetRandomTechs(3, PlayerStatus.GetTechs());
            }
            abilityAndTechSelection.DisplaySelection(availableAbilities, availableTechs);
            selectingAbility = true;
        }

        public void SelectAbilityAndTech(Ability ability, Tech tech)
        {
            PlayerStatus.AddAbility(ability);
            PlayerStatus.AddTech(tech);
            CurrentLevel.SetHasSelectedLevelAbility(true);
            abilityAndTechSelection.gameObject.SetActive(false);
            selectingAbility = false;
            availableAbilities.Clear();
            availableTechs.Clear();
            // Update health UI in case the ability affected player health
            playerHealthBar.UpdateHealth(PlayerStatus.Status.Health, PlayerStatus.Status.MaxHealth);
        }

        private void UpdateKeyCount()
        {
            keyCountText.text = PlayerStatus.KeyCount.ToString();
        }

        private void UpdateCurrentFloor()
        {
            currentFloorText.text = "Floor " + CurrentLevel.GetCurrentFloorNumber();
        }
    }
}
