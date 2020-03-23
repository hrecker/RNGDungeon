using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveDelay = 0.5f;
    public InventoryInput inventoryInput;
    private float timer;
    private bool moving;
    private PauseMenu pauseMenu;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    // This is just for testing individually the map scene
    private void Awake()
    {
        if (!GameInitialization.HasInitialized())
        {
            GameInitialization.StartNewGame();
        }
    }

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        this.transform.position = PlayerStatus.MapPosition;
    }

    void Update()
    {
        if (pauseMenu.IsPaused())
        {
            return;
        }

        if (moving)
        {
            timer += Time.deltaTime;
            Move();
        }
        else
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        int vertical = 0;
        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1;
        }

        int horizontal = 0;
        if (vertical == 0 && Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
        }
        else if (vertical == 0 && Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
        }

        if (horizontal != vertical && CurrentLevel.CheckMovement(
            ((Vector2)transform.position) + new Vector2(horizontal, vertical)))
        {
            startPosition = transform.position;
            targetPosition = startPosition + new Vector2(horizontal, vertical);
            moving = true;
            timer = 0.0f;
            // Inventory can't be opened while moving
            inventoryInput.SetInventoryEnabled(false);
        }
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(startPosition, targetPosition, timer / moveDelay);
        if (timer >= moveDelay)
        {
            moving = false;
            PlayerStatus.MapPosition = targetPosition;
            // Check for effects after the on-screen move is done
            MoveResult result = CurrentLevel.Move(targetPosition);
            switch (result)
            {
                case MoveResult.BATTLE:
                    SceneManager.LoadScene("BattleScene");
                    break;
                case MoveResult.NOTHING:
                    // Check input as soon as move is finished so there's no jittering
                    inventoryInput.SetInventoryEnabled(true);
                    CheckInput();
                    break;
            }
        }
    }
}
