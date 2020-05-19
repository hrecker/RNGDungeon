using UnityEngine.SceneManagement;
using Levels;
using Data;

public class GameInitialization
{
    private static bool hasInitialized;

    public static bool HasInitialized()
    {
        return hasInitialized;
    }

    public static void StartNewGame()
    {
        Level level1 = Cache.GetLevel(1);
        CurrentLevel.InitLevel(level1);
        PlayerStatus.Restart();
        PlayerController.ResetAbilityAndTechSelection();
        SceneManager.LoadScene("MapScene");
        hasInitialized = true;
    }
}
