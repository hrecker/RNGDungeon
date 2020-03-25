using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (ItemCount == 1)
        {
            itemCountText.enabled = false;
        }
        else
        {
            itemCountText.enabled = true;
            itemCountText.text = ItemCount.ToString();
        }
    }
}
