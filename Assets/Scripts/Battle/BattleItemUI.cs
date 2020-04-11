using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

namespace Battle
{
    public class BattleItemUI : MonoBehaviour
    {
        public RectTransform itemIconParent;
        public GameObject itemIconPrefab;
        public float marginBetweenItems;
        public BattleController battleController;

        private Dictionary<Item, GameObject> itemIcons;

        void Start()
        {
            // Create item icons for each item in the player's inventory
            //TODO handle duplicates
            itemIcons = new Dictionary<Item, GameObject>();
            CreateItemIcons();
            Render();
        }

        private void CreateItemIcons()
        {
            //TODO remove, for testing
            if (PlayerStatus.Inventory == null)
            {
                PlayerStatus.InitializeIfNecessary();
            }

            foreach (Item item in PlayerStatus.Inventory.Keys)
            {
                if (PlayerStatus.Inventory[item] > 0 &&
                    (item.itemType == ItemType.USABLE_ANYTIME ||
                    item.itemType == ItemType.USABLE_ONLY_IN_BATTLE))
                {
                    GameObject newIcon = InstantiateItemIcon(item,
                        itemIconPrefab, itemIconParent.transform, true);
                    itemIcons.Add(item, newIcon);
                }
            }
        }

        public static GameObject InstantiateItemIcon(Item item,
            GameObject itemIconPrefab, Transform itemIconParent, bool enableClick)
        {
            GameObject newIcon = Instantiate(itemIconPrefab, itemIconParent);
            Image itemImage = newIcon.GetComponent<Image>();
            Text tooltip = newIcon.GetComponentInChildren<Text>();
            ItemIcon icon = newIcon.GetComponent<ItemIcon>();
            itemImage.sprite = Data.Cache.GetItemIcon(item.name);
            tooltip.text = item.tooltipText;
            icon.ItemName = item.name;
            if (PlayerStatus.Inventory.ContainsKey(item))
            {
                icon.ItemCount = PlayerStatus.Inventory[item];
            }
            icon.clickEnabled = enableClick;
            return newIcon;
        }

        private void Render()
        {
            // Put items into the correct spot.
            float firstXPosition = (itemIcons.Keys.Count - 1f) / -2 * marginBetweenItems;
            int i = 0;
            foreach (Item item in itemIcons.Keys)
            {
                RectTransform rectTransform = itemIcons[item].GetComponent<RectTransform>();
                rectTransform.anchoredPosition =
                    Vector2.right * (firstXPosition + (i * marginBetweenItems));
                i++;
            }
            itemIconParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                (itemIcons.Keys.Count * marginBetweenItems));
        }

        public bool UseItem(string itemName)
        {
            Item found = null;
            foreach (Item item in itemIcons.Keys)
            {
                if (item.name == itemName)
                {
                    found = item;
                }
            }
            if (found == null)
            {
                return false;
            }

            // Apply item effects
            if (!battleController.ApplyPlayerItem(found))
            {
                return false;
            }

            // if player has no more of the given item, remove the icon and rerender
            if (!PlayerStatus.Inventory.ContainsKey(found) || PlayerStatus.Inventory[found] <= 0)
            {
                Destroy(itemIcons[found]);
                itemIcons.Remove(found);
                Render();
            }

            return true;
        }
    }
}
