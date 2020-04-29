using UnityEngine;
using UnityEngine.UI;
using Data;
using UI;

namespace Battle
{
    public class TechButton : MonoBehaviour
    {
        public string techName;
        public Text tooltipText;
        public Image techImage;
        public bool isDefaultTech;
        public GameObject cooldownPanel;
        public Text cooldownText;
        private Tech tech;
        private int lastRenderedCooldown;

        private TechButtonController buttonController;
        private PauseMenu pauseMenu;

        void Start()
        {
            pauseMenu = FindObjectOfType<PauseMenu>();
            buttonController = GetComponentInParent<TechButtonController>();
            buttonController.RegisterTechButton(this);
            lastRenderedCooldown = -1;

            if (isDefaultTech)
            {
                buttonController.SelectTech(this);
            }
            else
            {
                tech = Data.Cache.GetTech(techName);
                gameObject.name = techName;
                techImage.sprite = Data.Cache.GetTechIcon(techName);
                tooltipText.text = tech.GetDisplayName();
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

        private void UpdateCooldownText()
        {
            if (isDefaultTech || lastRenderedCooldown == tech.GetCurrentCooldown())
            {
                return;
            }

            int cooldown = tech.GetCurrentCooldown();
            cooldownPanel.SetActive(cooldown > 0);
            cooldownText.text = cooldown.ToString();
            lastRenderedCooldown = cooldown;
        }

        private void Update()
        {
            UpdateCooldownText();
        }
    }
}
