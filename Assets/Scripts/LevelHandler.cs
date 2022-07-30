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

    }

    private void Reset()
    {
        Map.ClearAllTiles();

        var level = LevelLoader.LoadLevel("testlevel");
        foreach (var platform in level.PlatformSpawnLocations)
            SpawnLayer(platform.x, platform.y, platform.size);
        PlayerController.Instance.transform.position = new Vector3(level.PlayerSpawnLocation.x + 0.5f, level.PlayerSpawnLocation.y + 1.5f);
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
