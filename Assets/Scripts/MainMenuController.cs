using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        GameInitialization.StartNewGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
