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

    public string[] mapRows =
    {
        "WWWWWWWW",
        "WPFFFCFW",
        "WFWFWFFW",
        "WFCFFWFW",
        "WFFWFFCW",
        "WFWFFWFW",
        "WCFFFFFW",
        "WWWWWWWW"
    };

    private Dictionary<Vector2Int, bool> walls = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, GameObject> coins = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int playerSpawnPoint;

    public int Width => mapRows[0].Length;
    public int Height => mapRows.Length;

    public void GenerateLevel()
    {
        ClearLevel();

        walls.Clear();
        coins.Clear();

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                char tile = mapRows[row][col];
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
                DestroyImmediate(environmentParent.GetChild(i).gameObject);
            }
        }

        if (coinsParent != null)
        {
            for (int i = coinsParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(coinsParent.GetChild(i).gameObject);
            }
        }
    }

    private Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3 (
            gridPos.x * tileSize, 
            0f, 
            -gridPos.y * tileSize
        );
    }
}