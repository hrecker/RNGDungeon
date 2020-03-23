using UnityEngine;
using UnityEngine.UI;

public abstract class BattleStatus : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    Text healthUIText;

    private int lastRenderCurrentHealth;

    void Start()
    {
        healthUIText = GetComponentInChildren<Text>();
        UpdateHealthText();
    }

    private void Update()
    {
        if (lastRenderCurrentHealth != currentHealth)
        {
            UpdateHealthText();
        }
    }

    private void UpdateHealthText()
    {
        healthUIText.text = currentHealth + "/" + maxHealth;
        lastRenderCurrentHealth = currentHealth;
    }

    public abstract void applyResult(RollResult rollResult);
}
