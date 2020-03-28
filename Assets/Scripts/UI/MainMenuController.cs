using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Load in enemy, level, item, etc. data before starting
        Cache.Load();
        GameInitialization.StartNewGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
