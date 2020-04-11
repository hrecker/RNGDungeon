using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ExitToMenuButton : MonoBehaviour
    {
        public void ExitToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
