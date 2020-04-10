using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject defaultButtonParent;
    public GameObject confirmButtonParent;
    public GameObject pauseMenuParent;

    private string selectedAction;
    private bool isPaused;

    private void Awake()
    {
        // Hide pause menu on awake
        HidePauseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        DisplayPauseMenu();
        isPaused = true;
        //TODO will need to disable any other inputs
        // PlayerController
        // Inventory
        // Battle stances (clicking on stances, tooltips)
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        HidePauseMenu();
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void DisplayPauseMenu()
    {
        pauseMenuParent.SetActive(true);
        defaultButtonParent.SetActive(true);
        confirmButtonParent.SetActive(false);
    }

    public void HidePauseMenu()
    {
        pauseMenuParent.SetActive(false);
    }

    public void Restart()
    {
        ConfirmActionPrompt("Restart");
    }

    public void ExitToMenu()
    {
        ConfirmActionPrompt("ExitToMenu");
    }

    public void ExitGame()
    {
        ConfirmActionPrompt("ExitGame");
    }

    public void ConfirmActionPrompt(string action)
    {
        selectedAction = action;
        defaultButtonParent.SetActive(false);
        confirmButtonParent.SetActive(true);
    }

    public void Confirm()
    {
        switch (selectedAction)
        {
            case "Restart":
                Unpause();
                GameInitialization.StartNewGame();
                break;
            case "ExitToMenu":
                Unpause();
                SceneManager.LoadScene("MainMenuScene");
                break;
            case "ExitGame":
                Application.Quit();
                break;
        }
    }

    public void Cancel()
    {
        selectedAction = "";
        defaultButtonParent.SetActive(true);
        confirmButtonParent.SetActive(false);
    }
}
