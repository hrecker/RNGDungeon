using UnityEngine;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        public void StartGame()
        {
            // Load in enemy, level, item, etc. data before starting
            Data.Cache.Load();
            GameInitialization.StartNewGame();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
