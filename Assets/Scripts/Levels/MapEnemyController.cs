using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnemyController : MonoBehaviour
{
    public float moveDelay = 0.15f;
    public float moveAttemptDelay = 0.1f;
    public float moveChance = 0.3f;
    private float timer;
    private bool moving;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (moving)
        {
            Move();
        }
        else
        {
            if (timer >= moveAttemptDelay)
            {
                if (Random.value < moveChance)
                {
                    StartMove();
                }
                timer = 0.0f;
            }
        }
    }
    private void Move()
    {
        transform.position = Vector3.Lerp(startPosition, targetPosition, timer / moveDelay);
        if (timer >= moveDelay)
        {
            startPosition = targetPosition;
            moving = false;
            timer = 0.0f;
        }
    }

    private void StartMove()
    {
        List<Vector2Int> movementOptions = new List<Vector2Int>();

        // Get list of available movement options
        foreach (Vector2Int option in new List<Vector2Int> { 
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
        {
            if (CurrentLevel.CheckEnemyMovement(transform.position + new Vector3(option.x, option.y)))
            {
                movementOptions.Add(option);
            }
        }

        // Move if possible
        if (movementOptions.Count > 0)
        {
            Vector2Int selected = movementOptions[Random.Range(0, movementOptions.Count)];
            moving = true;
            targetPosition = startPosition + selected;
            CurrentLevel.MoveCollector(startPosition, targetPosition);
        }
    }
}
