using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseAndInventoryInput : MonoBehaviour
{
    private bool inventoryEnabled;
    private bool isInInventory;

    void Start()
    {
        Scene active = SceneManager.GetActiveScene();
        inventoryEnabled = active.name == "MapScene";
        isInInventory = active.name == "InventoryScene";
    }

    void Update()
    {
        if (isInInventory && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)))
        {
            SceneManager.LoadScene("MapScene");
        }
        else if (inventoryEnabled && Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("InventoryScene");
        }
    }
}
