using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseAndInventoryInput : MonoBehaviour
{
    private bool openInventoryEnabled;
    private bool isInInventory;

    void Start()
    {
        Scene active = SceneManager.GetActiveScene();
        openInventoryEnabled = active.name == "MapScene";
        isInInventory = active.name == "InventoryScene";
    }

    public void SetInventoryEnabled(bool enabled)
    {
        openInventoryEnabled = enabled;
    }

    void Update()
    {
        if (isInInventory && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)))
        {
            SceneManager.LoadScene("MapScene");
        }
        else if (openInventoryEnabled && Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("InventoryScene");
        }
    }
}
