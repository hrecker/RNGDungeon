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

    protected virtual void Update()
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

    public abstract void ApplyResult(RollResult rollResult);

    public virtual void ApplyHealthChange(int diff)
    {
        currentHealth += diff;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
