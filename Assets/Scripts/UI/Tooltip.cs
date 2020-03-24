using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltip;

    private void Start()
    {
        tooltip.enabled = false;
    }

    // Add an eventtrigger to the UI object that needs a tooltip,
    // then point to these two methods for PointerEnter and PointerExit, respectively
    public void ShowTooltip()
    {
        tooltip.enabled = true;
    }

    public void HideTooltip()
    {
        tooltip.enabled = false;
    }
}
