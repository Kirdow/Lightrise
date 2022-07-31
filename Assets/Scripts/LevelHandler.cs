using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelHandler : MonoBehaviour
{
    [Header("General")]
    public Tile[] PlatformTiles = new Tile[3];
    public Tile FeatherTile;

    [Header("Debug")]
    public Tile Pixel;

    [HideInInspector]
    public Tilemap Map;

    public int LastCameraY { get; private set; }

    private CameraController _cameraController;
    private LevelData _levelData;


    void Awake()
    {
        if (Instance == null) Instance = this;

        Map = GetComponent<Tilemap>();
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        int minY = _cameraController.Bound.SafeMinTile;
        if (LastCameraY < minY)
        {
            for (int y = LastCameraY + 1; y <= minY; y++)
            {
                for (int x = 0; x < _levelData.Width; x++)
                {
                    SetTile(x, y, null);
                }
            }

            LastCameraY = minY;
        }

        if (PlayerController.Instance.transform.position.y < minY - 1.0f)
        {
            FadedOverlay.Instance.ExecuteFadeSequence(() => Reset());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void ResetWithLevel(string levelName)
    {
        Map.ClearAllTiles();

        LevelData level = LevelLoader.LoadLevel(levelName);
        _levelData = level;

        foreach (var platform in level.PlatformSpawnLocations)
            SpawnLayer(platform.x, platform.y, platform.size);

        PlayerController.Instance.transform.position = new Vector3(level.PlayerSpawnLocation.x + 0.5f, level.PlayerSpawnLocation.y + PlayerController.Instance.transform.localScale.y / 2.0f + 0.05f);
    }

    private void Reset()
    {
        ResetWithLevel("testlevel");

        // This will be accurately set during frame/update
        LastCameraY = -10;
        _cameraController.Reset();
        PlayerController.Instance.Reset();
    }

    public void SpawnLayer(int x, int y, int width)
    {
        int start = x - width;
        int end = x + width;

        for (int xpos = start; xpos < end; xpos++)
        {
            var tile = xpos == start ? PlatformTiles[0] : (xpos == end - 1 ? PlatformTiles[2] : PlatformTiles[1]);
            SetTile(xpos, y, tile);
        }
    }

    private void SetTile(int x, int y, Tile tile)
    {
        if (y < 0) return;
        if (x < 0 || x >= _levelData.Width || y >= _levelData.Height)
        {
             Debug.LogError("TILE OUT OF BOUNDS");
        }

        Map.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public static LevelHandler Instance { get; private set; }
}
