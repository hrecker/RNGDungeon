using UnityEngine;
using UnityEngine.UI;

public class TechButton : MonoBehaviour
{
    public string techName;
    public Text tooltipText;
    public Image techImage;
    public bool isDefaultTech;
    public GameObject cooldownPanel;
    public Text cooldownText;
    private Tech tech;

    private TechButtonController buttonController;
    private PauseMenu pauseMenu;
    private int currentCooldown;

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        buttonController = GetComponentInParent<TechButtonController>();
        buttonController.RegisterTechButton(this);
        if (cooldownPanel != null)
        {
            cooldownPanel.SetActive(false);
        }

        if (isDefaultTech)
        {
            buttonController.SelectTech(this);
        }
        else
        {
            tech = Cache.GetTech(techName);
            gameObject.name = techName;
            techImage.sprite = Cache.GetTechIcon(techName);
            tooltipText.text = tech.tooltip;
        }
    }

    public void OnSelected()
    {
        if (!pauseMenu.IsPaused() && currentCooldown <= 0)
        {
            buttonController.SelectTech(this);
        }
    }

    public void DecrementCooldownIfNecessary()
    {
        if (currentCooldown > 0)
        {
            currentCooldown--;
        }
        else
        {
            return;
        }

        if (currentCooldown > 0)
        {
            UpdateCooldownText();
        }
        else
        {
            cooldownPanel.SetActive(false);
        }
    }

    public void ActivateCooldown()
    {
        // Default tech has no cooldown
        if (isDefaultTech)
        {
            return;
        }

        cooldownPanel.SetActive(true);
        currentCooldown = tech.cooldownRolls;
        UpdateCooldownText();
    }

    private void UpdateCooldownText()
    {
        cooldownText.text = currentCooldown.ToString();
    }
}
