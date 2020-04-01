using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public RectTransform redBar;
    public Text healthText;

    void Start()
    {
        UpdateHealthBar();
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthText.text = health + "/" + maxHealth;
        if (maxHealth != 0)
        {
            float redHealthPercentage = 200 *
                (maxHealth - health) / maxHealth;
            redBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, redHealthPercentage);
        }
    }
}
