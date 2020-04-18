using UnityEngine;
using UI;

namespace Battle
{
    public class BattleHealthBar : MonoBehaviour
    {
        public BattleStatus status;
        HealthBar healthBar;

        private int lastRenderHealth;

        void Start()
        {
            healthBar = GetComponentInChildren<HealthBar>();
            UpdateHealthBar();
        }

        private void Update()
        {
            if (lastRenderHealth != status.Health)
            {
                UpdateHealthBar();
            }
        }

        private void UpdateHealthBar()
        {
            healthBar.UpdateHealth(status.Health, status.MaxHealth);
            lastRenderHealth = status.Health;
        }

        public void ApplyResult(RollResult rollResult)
        {
            status.Health += rollResult.GetTotalHealthChange(status.Actor);
        }
    }
}
