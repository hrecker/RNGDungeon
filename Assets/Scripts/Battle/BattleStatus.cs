using UnityEngine;
using UI;

namespace Battle
{
    public abstract class BattleStatus : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        HealthBar healthBar;

        private int lastRenderCurrentHealth;

        void Start()
        {
            healthBar = GetComponentInChildren<HealthBar>();
            UpdateHealthBar();
        }

        protected virtual void Update()
        {
            if (lastRenderCurrentHealth != currentHealth)
            {
                UpdateHealthBar();
            }
        }

        private void UpdateHealthBar()
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
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
}
