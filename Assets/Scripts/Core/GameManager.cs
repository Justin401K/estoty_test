using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GridGenerator gridGenerator;
    public UIManager uiManager;
    public AudioManager audioManager;

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

        uiManager.SetCoinText(collectedCoins, totalCoins);
        uiManager.ShowWinMessage(false);
    }

    public void HandlePlayerSteppedOnTile(Vector2Int gridPos)
    {
        if (audioManager != null)
        {
            audioManager.PlayStep();
        }

        if (gridGenerator.HasCoin(gridPos))
        {
            gridGenerator.CollectCoin(gridPos);
            collectedCoins++;

            uiManager.SetCoinText(collectedCoins, totalCoins);

            if (audioManager != null)
            {
                audioManager.PlayCoin();
            }

            if (collectedCoins == totalCoins)
            {
                IsGameWon = true;
                uiManager.ShowWinMessage(true);

                if (audioManager != null)
                {
                    audioManager.PlayWin();
                }
            }
        }
    }

    public void RestartLevel()
    {
        StartNewGame();
    }
}