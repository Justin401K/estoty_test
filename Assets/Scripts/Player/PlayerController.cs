using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveDuration = 0.18f;

    private GridGenerator grid;
    private GameManager gameManager;

    private Vector2Int currentGridPos;
    private bool isMoving = false;

    private Animator animator;

    public void Initialize(GridGenerator gridGenerator, GameManager manager, Vector2Int startPos)
    {
        grid = gridGenerator;
        gameManager = manager;
        currentGridPos = startPos;
        transform.position = grid.GridToWorld(currentGridPos);
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isMoving || gameManager == null || gameManager.IsGameWon)
        {
            return;
        }

        Vector2Int input = GetInputDirection();

        if (input != Vector2Int.zero)
        {
            TryMove(input);
        }
    }

    private Vector2Int GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Vector2Int.left;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Vector2Int.right;
        }

        return Vector2Int.zero;
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int targetPos = currentGridPos + direction;

        if (!grid.IsInsideGrid(targetPos))
        {
            return;
        }

        if (grid.IsWall(targetPos))
        {
            return;
        }

        StartCoroutine(SmoothMove(targetPos, direction));
    }

    private IEnumerator SmoothMove(Vector2Int targetGridPos, Vector2Int direction)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = grid.GridToWorld(targetGridPos);

        RotateToDirection(direction);

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        currentGridPos = targetGridPos;

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }

        gameManager.HandlePlayerSteppedOnTile(currentGridPos);

        isMoving = false;
    }

    private void RotateToDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (direction == Vector2Int.down)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (direction == Vector2Int.left)
        {
            transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }
        else if (direction == Vector2Int.right)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }

    public void ResetPosition(Vector2Int startPos)
    {
        StopAllCoroutines();
        isMoving = false;
        currentGridPos = startPos;
        transform.position = grid.GridToWorld(currentGridPos);

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }
    }

}