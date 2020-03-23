using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StanceButton : MonoBehaviour
{
    public string stanceName;
    public Color selectedColor = Color.green;
    public Color unselectedColor = Color.white;

    private StanceButtonController buttonController;
    private Image buttonImage;
    private Text tooltip;

    void Start()
    {
        buttonController = GetComponentInParent<StanceButtonController>();
        buttonImage = GetComponent<Image>();
        foreach (Text child in GetComponentsInChildren<Text>())
        {
            if (child.name == "Tooltip")
            {
                tooltip = child;
                break;
            }
        }
        buttonController.RegisterStanceButton(this);

        if (stanceName == PlayerStatus.SelectedStance)
        {
            OnSelected();
        }
    }

    public void OnSelected()
    {
        buttonController.SelectStance(this);
        buttonImage.color = selectedColor;
    }

    public void SetUnselected()
    {
        buttonImage.color = unselectedColor;
    }

    public void ShowTooltip()
    {
        tooltip.enabled = true;
    }

    public void HideTooltip()
    {
        tooltip.enabled = false;
    }
}
