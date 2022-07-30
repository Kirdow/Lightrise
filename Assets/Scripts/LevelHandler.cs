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

    public int LastPlatform { get; private set; }
    public int PlatformGap { get; private set; }
    public int LastPlatformId { get; private set; }

    private CameraController _cameraController;


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
        while (LastPlatform <= Mathf.FloorToInt(_cameraController.MaxBound - PlatformGap))
        {
            LastPlatform += PlatformGap;
            LastPlatformId += 1;
            SpawnLayer((LastPlatformId % 3 - 1) * 2, LastPlatform, 5);
        }
    }

    private void Reset()
    {
        Map.ClearAllTiles();

        PlatformGap = 5;
        LastPlatform = -5;
        while (true)
        {
            SpawnLayer(LastPlatform / 2, LastPlatform, 5);
            if (LastPlatform == 5) break;
            LastPlatform += PlatformGap;
        }
        for (int i = 0; i < 1; i++)
            Map.SetTile(new Vector3Int(-5, -4 + i, 0), Pixel);
        LastPlatformId = 3;
    }

    public void SpawnLayer(int x, int y, int width)
    {
        int start = x - width;
        int end = x + width;

        for (int xpos = start; xpos < end; xpos++)
        {
            var tile = xpos == start ? PlatformTiles[0] : (xpos == end - 1 ? PlatformTiles[2] : PlatformTiles[1]);
            Map.SetTile(new Vector3Int(xpos, y, 0), tile);
        }
    }

    public static LevelHandler Instance { get; private set; }
}
