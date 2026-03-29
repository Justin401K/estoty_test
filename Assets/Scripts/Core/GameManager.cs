using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GridGenerator gridGenerator;

    private PlayerController player;
    private int totalCoins;
    private int collectedCoins;

    public bool IsGameWon { get; private set; }

    private void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        IsGameWon = false;
        collectedCoins = 0;

        gridGenerator.GenerateLevel();
        totalCoins = gridGenerator.Coins.Count;

        if (player == null)
        {
            GameObject playerObj = Instantiate(
                gridGenerator.playerPrefab,
                gridGenerator.GridToWorld(gridGenerator.PlayerSpawnPoint),
                Quaternion.identity
            );

            player = playerObj.GetComponent<PlayerController>();
        }

        player.Initialize(gridGenerator, this, gridGenerator.PlayerSpawnPoint);   
    }

    public void HandlePlayerSteppedOnTile(Vector2Int gridPos)
    {
        if (gridGenerator.HasCoin(gridPos))
        {
            gridGenerator.CollectCoin(gridPos);
            collectedCoins++;

            if (collectedCoins == totalCoins)
            {
                IsGameWon = true;
            }
        }
    }

    public void RestartLevel()
    {
        StartNewGame();
    }
}