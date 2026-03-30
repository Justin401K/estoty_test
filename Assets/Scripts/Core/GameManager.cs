using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GridGenerator gridGenerator;
    public UIManager uiManager;

    private PlayerController player;
    private int totalCoins;
    private int collectedCoins;
    private int currentLevelIndex = 0;

    public bool IsGameWon { get; private set; }

    private void Start()
    {
        StartLevel(currentLevelIndex);
    }

    public void StartLevel(int levelIndex)
    {
        IsGameWon = false;
        collectedCoins = 0;
        currentLevelIndex = levelIndex;

        gridGenerator.GenerateLevel(currentLevelIndex);
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

        uiManager.SetCoinText(collectedCoins, totalCoins);
        uiManager.SetLevelText(currentLevelIndex + 1, gridGenerator.LevelCount);
        uiManager.ShowWinMessage(false);
    }

    public void HandlePlayerSteppedOnTile(Vector2Int gridPos)
    {
        if (gridGenerator.HasCoin(gridPos))
        {
            gridGenerator.CollectCoin(gridPos);
            collectedCoins++;

            uiManager.SetCoinText(collectedCoins, totalCoins);

            if (collectedCoins == totalCoins)
            {
                if (currentLevelIndex < gridGenerator.LevelCount - 1)
                {
                    StartLevel(currentLevelIndex + 1);
                }
                else
                {
                    IsGameWon = true;
                    uiManager.ShowWinMessage(true);
                }
            }
        }
    }

    public void RestartLevel()
    {
        StartLevel(0);
    }
}