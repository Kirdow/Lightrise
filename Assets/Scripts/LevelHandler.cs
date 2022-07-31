using System;
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

    private bool _playActive = false;
    private string _levelName = "level_0";
    private string _nextLevel = null;
    private bool _winActive = false;


    void Awake()
    {
        if (Instance == null) Instance = this;

        Map = GetComponent<Tilemap>();
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    private string GetLevelForId(int id)
    {
        return _levelName.Substring(0, _levelName.IndexOf('_')) + "_" + id;
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] args = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(args, "--load-level");
        if (index >= 0 && index + 1 < args.Length)
        {
            string levelName = args[index + 1];
            if (LevelLoader.IsValidExternalLevel(levelName))
            {
                _levelName = levelName;
                Debug.Log($"Loading external level: ({levelName})");
            }    
        }

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            HelpText.ToggleHelpText();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (_playActive && !_winActive)
        {
            ClearPassedTiles();
            CheckLoss();
            CheckReset();
        }
        else if (_winActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R)))
        {
            _levelName = GetLevelForId(0);
            WinText.SetVisible(false);
            ResetWithFade(true);
        }
    }

    private void ClearPassedTiles()
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
    }

    private void CheckLoss()
    {
        int minY = _cameraController.Bound.SafeMinTile;
        if (PlayerController.Instance.transform.position.y < minY)
        {
            _playActive = false;
            SoundManager.PlaySound(GameAssets.I.deathAudio);
            FadedOverlay.Instance.ExecuteFadeSequence(() => Reset(false), () => _playActive = true);
        }
    }

    private void CheckReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void ResetWithLevel(string levelName, bool play = true)
    {
        Map.ClearAllTiles();
        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(item);
        }

        // This will be accurately set during frame/update
        LastCameraY = -10;
        _cameraController.Reset();

        LevelData level = LevelLoader.LoadLevel(levelName);
        _levelData = level;
        PlayerController.Instance.Reset(level);

        foreach (var platform in level.PlatformSpawnLocations)
            SpawnLayer(platform.x, platform.y, platform.size);

        PlayerController.Instance.transform.position = new Vector3(level.PlayerSpawnLocation.x + 0.5f, level.PlayerSpawnLocation.y + PlayerController.Instance.transform.localScale.y / 2.0f + 0.05f);
        if (level.NextLevel >= 0)
        {
            _nextLevel = GetLevelForId(level.NextLevel);
            LightItem.SummonAt(level.LightSpawnLocation.x, level.LightSpawnLocation.y, () =>
            {
                _levelName = _nextLevel;
                ResetWithFade();
            });
        }
        else
        {
            _nextLevel = null;
            ModuleItem.SummonAt(level.LightSpawnLocation.x, level.LightSpawnLocation.y, () =>
            {
                _playActive = false;
                FadedOverlay.Instance.ExecuteFadeSequence(() => Win(), null, false);
            });
        }

        if (play)
            _playActive = true;
    }

    public void ResetWithFade(bool onlyFadeIn = false)
    {
        _playActive = false;
        if (onlyFadeIn)
        {
            Reset(false);
            FadedOverlay.Instance.ExecuteFadeInSequence(() => _playActive = true);
            return;
        }

        FadedOverlay.Instance.ExecuteFadeSequence(() => Reset(false), () => _playActive = true);
    }

    private void Win()
    {
        _winActive = true;
        WinText.SetVisible(true);
    }

    private void Reset(bool play = true)
    {
        ResetWithLevel(_levelName, play);
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
