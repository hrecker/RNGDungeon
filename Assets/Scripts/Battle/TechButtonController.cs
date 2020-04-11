using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Battle
{
    public class TechButtonController : MonoBehaviour
    {
        public RectTransform selectedHighlight;
        public Vector2 selectedLargeSize;
        public Vector2 selectedSmallSize;
        public float techButtonSpacing;
        public GameObject techButtonPrefab;

        private List<TechButton> buttons;
        private TechButton defaultTech;
        private TechButton selectedTech;

        private void Awake()
        {
            buttons = new List<TechButton>();
            // Create buttons based on enabled techs for the player
            int techCount = 0;
            int techUiSide = -1;
            foreach (Tech playerTech in PlayerStatus.EnabledTechs)
            {
                float techXPos = ((techCount / 2) + 1) * techButtonSpacing * techUiSide;
                GameObject newButtonObject = Instantiate(techButtonPrefab, this.transform);
                TechButton newButton = newButtonObject.GetComponent<TechButton>();
                RectTransform newButtonTransform = newButtonObject.GetComponent<RectTransform>();

                newButton.techName = playerTech.name;
                newButtonTransform.anchoredPosition = new Vector2(techXPos, 0);

                techCount++;
                techUiSide *= -1;
            }
        }

        public void RegisterTechButton(TechButton button)
        {
            if (button.isDefaultTech)
            {
                defaultTech = button;
            }
            buttons.Add(button);
        }

        public void SelectTech(TechButton button)
        {
            selectedTech = button;
            // Move and resize the selected highlight
            selectedHighlight.anchoredPosition =
                button.GetComponent<RectTransform>().anchoredPosition;
            float selectedWidth = selectedSmallSize.x;
            float selectedHeight = selectedSmallSize.y;
            if (button.isDefaultTech)
            {
                selectedWidth = selectedLargeSize.x;
                selectedHeight = selectedLargeSize.y;
            }
            selectedHighlight.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selectedWidth);
            selectedHighlight.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selectedHeight);
        }

        // Trigger cooldown update on tech buttons
        public void Roll()
        {
            // Cooldowns
            foreach (TechButton button in buttons)
            {
                button.DecrementCooldownIfNecessary();
            }
            // Reselect default if necessary
            if (selectedTech != defaultTech)
            {
                selectedTech.ActivateCooldown();
                SelectTech(defaultTech);
            }
        }

        public Tech GetSelectedTech()
        {
            return Data.Cache.GetTech(selectedTech.name);
        }
    }
}
