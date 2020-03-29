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

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        buttonController = GetComponentInParent<TechButtonController>();
        buttonController.RegisterTechButton(this);

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
            UpdateCooldownText();
        }
    }

    public void OnSelected()
    {
        if (!pauseMenu.IsPaused() && tech.GetCurrentCooldown() <= 0)
        {
            buttonController.SelectTech(this);
        }
    }

    public void DecrementCooldownIfNecessary()
    {
        if (tech != null && tech.GetCurrentCooldown() > 0)
        {
            tech.DecrementCooldown();
        }
        else
        {
            return;
        }

        UpdateCooldownText();
    }

    public void ActivateCooldown()
    {
        // Default tech has no cooldown
        if (isDefaultTech)
        {
            return;
        }

        tech.ActivateCooldown();
        UpdateCooldownText();
    }

    private void UpdateCooldownText()
    {
        if (isDefaultTech)
        {
            return;
        }

        int cooldown = tech.GetCurrentCooldown();
        cooldownPanel.SetActive(cooldown > 0);
        cooldownText.text = cooldown.ToString();
    }
}
