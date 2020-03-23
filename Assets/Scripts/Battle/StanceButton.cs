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
    private Button button;
    private PauseMenu pauseMenu;

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        buttonController = GetComponentInParent<StanceButtonController>();
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
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

    private void Update()
    {
        // Disable button when paused
        button.enabled = !pauseMenu.IsPaused();
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
