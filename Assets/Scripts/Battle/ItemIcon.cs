using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class ItemIcon : MonoBehaviour
    {
        private BattleItemUI itemUIController;
        // Index of this icon in the scroll panel
        public string ItemName { get; set; }
        public int ItemCount { get; set; }
        public Text itemCountText;
        public bool clickEnabled = true;

        private void Start()
        {
            if (clickEnabled)
            {
                itemUIController = GameObject.Find("ItemScrollPanel").GetComponent<BattleItemUI>();
            }
            UpdateItemCount();
        }

        public void OnClick()
        {
            if (clickEnabled && itemUIController.UseItem(ItemName))
            {
                ItemCount--;
                UpdateItemCount();
                // if this icon should be deleted, the battleitemui will handle that
            }
        }

        private void UpdateItemCount()
        {
            itemCountText.enabled = true;
            itemCountText.text = ItemCount.ToString();
        }
    }
}
