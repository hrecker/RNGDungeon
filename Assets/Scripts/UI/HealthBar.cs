using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public RectTransform greenBar;
    private float maxWidth;
    public RectTransform redBar;
    public Text healthText;

    void Start()
    {
        maxWidth = greenBar.rect.width;
        UpdateHealthBar();
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthText.text = health + "/" + maxHealth;
        if (maxHealth != 0)
        {
            float redHealthPercentage = maxWidth *
                (maxHealth - health) / maxHealth;
            redBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, redHealthPercentage);
        }
    }
}
