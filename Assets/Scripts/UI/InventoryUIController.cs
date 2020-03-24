using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public GameObject textPrefab;
    public RectTransform redHealthBar;
    public Text healthText;
    public Transform statusBase;
    public Transform equipmentBase;
    public Transform abilityBase;
    public Transform inventoryBase;

    public int lineSeparation = 25;

    void Start()
    {
        UpdateHealthUI();
        UpdateStatusUI();
        UpdateEquipmentUI();
        UpdateAbilityUI();
        UpdateInventoryUI();
    }

    private void UpdateHealthUI()
    {
        // For testing
        if (PlayerStatus.MaxHealth == 0)
        {
            PlayerStatus.Health = 10;
            PlayerStatus.MaxHealth = 10;
        }

        healthText.text = PlayerStatus.Health + "/" + PlayerStatus.MaxHealth;
        float redHealthPercentage = 100 *
            (PlayerStatus.MaxHealth - PlayerStatus.Health) / PlayerStatus.MaxHealth;
        redHealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, redHealthPercentage);
    }

    private void UpdateStatusUI()
    {
        //TODO get statuses from player status once they are added there
        List<string> statuses = new List<string>() { "Poisoned", "++MaxRoll" };
        UpdateListUI(statuses, statusBase);
    }

    private void UpdateEquipmentUI()
    {
        //TODO get equipment from player status once they are added there
        List<string> equipment = new List<string>() { "GRU", "Dragon Defender" };
        UpdateListUI(equipment, equipmentBase);
    }

    private void UpdateAbilityUI()
    {
        //TODO get abilities from player status once they are added there
        List<string> abilities = new List<string>() { "High Roller", "Second Chance" };
        UpdateListUI(abilities, abilityBase);
    }

    private void UpdateInventoryUI()
    {
        //TODO get inventory from player status once they are added there
        List<string> inventory = new List<string>() { "Small Health Potion", "Potion of Protection", "Banana" };
        UpdateListUI(inventory, inventoryBase);
    }

    private void UpdateListUI(List<string> displayValues, Transform uiParent)
    {
        for (int i = 0; i < displayValues.Count; i++)
        {
            GameObject newText = Instantiate(textPrefab, uiParent);
            //Instantiate()
            //newText.transform.SetParent(uiParent);
            newText.GetComponent<Text>().text = displayValues[i];
            newText.GetComponent<RectTransform>().Translate(i * lineSeparation * Vector3.down);
        }
    }
}
