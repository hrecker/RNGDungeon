using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
        public GameObject tooltipPanel;
        public Text tooltipText;
        public bool disabled;

        private void Start()
        {
            HideTooltip();
        }

        public void SetTooltipText(string text)
        {
            tooltipText.text = text;
        }

        // Add an eventtrigger to the UI object that needs a tooltip,
        // then point to these two methods for PointerEnter and PointerExit, respectively
        public void ShowTooltip()
        {
            if (!disabled)
            {
                if (tooltipPanel != null)
                {
                    tooltipPanel.SetActive(true);
                }
                tooltipText.enabled = true;
            }
        }

        public void HideTooltip()
        {
            if (tooltipPanel != null)
            {
                tooltipPanel.SetActive(false);
            }
            tooltipText.enabled = false;
        }

        public void SetEnabled(bool enabled)
        {
            this.disabled = !enabled;
            if (disabled)
            {
                HideTooltip();
            }
        }
    }
}
