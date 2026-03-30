using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public float tileSize = 1f;

    [Header("Prefabs")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject coinPrefab;
    public GameObject playerPrefab;

    [Header("Parents")]
    public Transform environmentParent;
    public Transform coinsParent;

    private readonly string[][] levels =
    {
        new string[]
        {
            "WWWWWWWW",
            "WPFFFCFW",
            "WFWFWWFW",
            "WFCFFWFW",
            "WFWWFFCW",
            "WFWFFWFW",
            "WCFFFFFW",
            "WWWWWWWW"
        },
        new string[]
        {
            "WWWWWWWW",
            "WPFWFFCW",
            "WFWFWWFW",
            "WFFCFWFW",
            "WCWWFFFW",
            "WFFFWFCW",
            "WCFWFFFW",
            "WWWWWWWW"
        },
        new string[]
        {
            "WWWWWWWW",
            "WPFCFFFW",
            "WWFWFWFW",
            "WFFWFCFW",
            "WCFWFFWW",
            "WFFFCFFW",
            "WFWFWCFW",
            "WWWWWWWW"
        },
        new string[]
        {
            "WWWWWWWW",
            "WPFFFFCW",
            "WFWCWWFW",
            "WCFFFWFW",
            "WFWWWFFW",
            "WFFCFWFW",
            "WCFWFFFW",
            "WWWWWWWW"
        },
        new string[]
        {
            "WWWWWWWW",
            "WPFWCFFW",
            "WFFWFWCW",
            "WWFCFWFW",
            "WCFFWWFW",
            "WFWCFFFW",
            "WFFFWCFW",
            "WWWWWWWW"
        }
    };

    private Dictionary<Vector2Int, bool> walls = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, GameObject> coins = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int playerSpawnPoint;
    private string[] currentMapRows;

    public int Width => currentMapRows[0].Length;
    public int Height => currentMapRows.Length;

    public IReadOnlyDictionary<Vector2Int, bool> Walls => walls;
    public IReadOnlyDictionary<Vector2Int, GameObject> Coins => coins;
    public Vector2Int PlayerSpawnPoint => playerSpawnPoint;
    public int LevelCount => levels.Length;

    public void GenerateLevel(int levelIndex)
    {
        ClearLevel();

        walls.Clear();
        coins.Clear();

        currentMapRows = levels[levelIndex];

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                char tile = currentMapRows[row][col];
                Vector3 worldPos = GridToWorld(new Vector2Int(col, row));

                Instantiate(floorPrefab, worldPos, Quaternion.identity, environmentParent);

                switch (tile)
                {
                    case 'W':
                        Instantiate(wallPrefab, worldPos, Quaternion.identity, environmentParent);
                        walls[new Vector2Int(col, row)] = true;
                        break;

                    case 'C':
                        GameObject coin = Instantiate(coinPrefab, worldPos + Vector3.up * 0.5f, Quaternion.identity, coinsParent);
                        coins[new Vector2Int(col, row)] = coin;
                        break;

                    case 'P':
                        playerSpawnPoint = new Vector2Int(col, row);
                        break;
                }
            }
        }
    }

    private void ClearLevel()
    {
        if (environmentParent != null)
        {
            for (int i = environmentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(environmentParent.GetChild(i).gameObject);
            }
        }

        if (coinsParent != null)
        {
            for (int i = coinsParent.childCount - 1; i >= 0; i--)
            {
                Destroy(coinsParent.GetChild(i).gameObject);
            }
        }
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * tileSize,
            0f,
            -gridPos.y * tileSize
        );
    }

    public bool IsInsideGrid(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < Width && gridPos.y >= 0 && gridPos.y < Height;
    }

    public bool IsWall(Vector2Int gridPos)
    {
        return walls.ContainsKey(gridPos) && walls[gridPos];
    }

    public bool HasCoin(Vector2Int gridPos)
    {
        return coins.ContainsKey(gridPos) && coins[gridPos] != null;
    }

    public void CollectCoin(Vector2Int gridPos)
    {
        if (HasCoin(gridPos))
        {
            Destroy(coins[gridPos]);
            coins.Remove(gridPos);
        }
    }
}