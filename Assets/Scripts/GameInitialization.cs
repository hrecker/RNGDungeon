using UnityEngine.SceneManagement;

public class GameInitialization
{
    private static bool hasInitialized;

    public static bool HasInitialized()
    {
        return hasInitialized;
    }

    public static void StartNewGame()
    {
        CurrentLevel.generateLevel(10, 10);
        PlayerStatus.Restart();
        SceneManager.LoadScene("MapScene");
        hasInitialized = true;
    }
}
