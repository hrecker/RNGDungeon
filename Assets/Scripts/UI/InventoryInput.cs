using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class InventoryInput : MonoBehaviour
    {
        private PauseMenu pauseMenu;
        private bool openInventoryEnabled;
        private bool isInInventory;

        void Start()
        {
            pauseMenu = FindObjectOfType<PauseMenu>();
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
            // Don't accept input while paused
            if (pauseMenu.IsPaused())
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isInInventory)
                {
                    SceneManager.LoadScene("MapScene");
                }
                else if (openInventoryEnabled)
                {
                    SceneManager.LoadScene("InventoryScene");
                }
            }
        }
    }
}
