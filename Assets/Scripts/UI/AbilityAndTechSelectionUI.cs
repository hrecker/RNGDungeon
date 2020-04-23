using UnityEngine;
using UnityEngine.UI;
using Levels;
using Data;
using System.Collections.Generic;

namespace UI
{
    // Handles player selection between three abilities and techs at beginning of floor
    public class AbilityAndTechSelectionUI : MonoBehaviour
    {
        public PlayerController playerController;

        public Image[] abilityImages;
        public Text[] abilityNames;
        public Text abilityDescription;
        public Image abilitySelectHighlight;

        public Image[] techImages;
        public Text[] techNames;
        public Text[] techCooldowns;
        public Text techDescription;
        public Image techSelectHighlight;

        private List<Ability> abilities;
        private List<Tech> techs;
        private static int currentAbilitySelection;
        private static int currentTechSelection;        

        public void DisplaySelection(List<Ability> abilities, List<Tech> techs)
        {
            if (abilities.Count != 3 || techs.Count != 3)
            {
                throw new System.Exception("Invalid ability or tech selections - 3 each are required");
            }

            this.abilities = abilities;
            this.techs = techs;
            SelectAbility(currentAbilitySelection);
            SelectTech(currentTechSelection);

            for (int i = 0; i < abilities.Count; i++)
            {
                abilityImages[i].sprite = Data.Cache.GetAbilityIcon(abilities[i].name);
                abilityNames[i].text = abilities[i].GetDisplayName();
            }
            for (int i = 0; i < techs.Count; i++)
            {
                techImages[i].sprite = Data.Cache.GetTechIcon(techs[i].name);
                techNames[i].text = techs[i].GetDisplayName();
                techCooldowns[i].text = techs[i].GetBaseCooldown().ToString();
            }
        }

        public void ShowAbilityDescription(int index)
        {
            abilityDescription.text = abilities[index].description;
        }

        public void ShowTechDescription(int index)
        {
            techDescription.text = techs[index].description;
        }

        public void HideAbilityDescription()
        {
            abilityDescription.text = "";
        }

        public void HideTechDescription()
        {
            techDescription.text = "";
        }

        public void SelectAbility(int index)
        {
            currentAbilitySelection = index;
            abilitySelectHighlight.transform.SetParent(abilityImages[index].transform);
            abilitySelectHighlight.rectTransform.anchoredPosition = Vector2.zero;
        }

        public void SelectTech(int index)
        {
            currentTechSelection = index;
            techSelectHighlight.transform.SetParent(techImages[index].transform);
            techSelectHighlight.rectTransform.anchoredPosition = Vector2.zero;
        }

        public void ConfirmSelections()
        {
            playerController.SelectAbilityAndTech(
                abilities[currentAbilitySelection],
                techs[currentTechSelection]);
            currentAbilitySelection = 0;
            currentTechSelection = 0;
        }
    }
}
