using UnityEngine;
using UnityEngine.UI;
using Levels;
using Data;

namespace UI
{
    public class AbilitySelectionUI : MonoBehaviour
    {
        public PlayerController playerController;
        public Image ability1Image;
        public Image ability2Image;
        public Image ability3Image;
        public Text ability1Name;
        public Text ability2Name;
        public Text ability3Name;
        public Text abilityDescription;

        private Ability ability1;
        private Ability ability2;
        private Ability ability3;

        public void DisplayAbilitySelection(Ability ability1, Ability ability2, Ability ability3)
        {
            this.ability1 = ability1;
            this.ability2 = ability2;
            this.ability3 = ability3;

            ability1Image.sprite = Data.Cache.GetAbilityIcon(ability1.name);
            ability2Image.sprite = Data.Cache.GetAbilityIcon(ability2.name);
            ability3Image.sprite = Data.Cache.GetAbilityIcon(ability3.name);
            ability1Name.text = ability1.GetDisplayName();
            ability2Name.text = ability2.GetDisplayName();
            ability3Name.text = ability3.GetDisplayName();
        }

        public void ShowDescription(int abilityIndex)
        {
            switch (abilityIndex)
            {
                case 1:
                    abilityDescription.text = ability1.description;
                    break;
                case 2:
                    abilityDescription.text = ability2.description;
                    break;
                case 3:
                    abilityDescription.text = ability3.description;
                    break;
            }
        }

        public void HideDescription()
        {
            abilityDescription.text = "";
        }

        public void SelectAbility(int abilityIndex)
        {
            switch (abilityIndex)
            {
                case 1:
                    playerController.SelectAbility(ability1);
                    break;
                case 2:
                    playerController.SelectAbility(ability2);
                    break;
                case 3:
                    playerController.SelectAbility(ability3);
                    break;
            }
        }
    }
}
