using UnityEngine;

public class LevelData
{
    public string LevelName { get; private set; }

    public PlatformSpawn[] PlatformSpawnLocations { get; private set; }
    public PlayerSpawn PlayerSpawnLocation { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public LevelData(string levelName, PlatformSpawn[] platforms, PlayerSpawn playerSpawn, int width, int height)
        => (LevelName, PlatformSpawnLocations, PlayerSpawnLocation, Width, Height) = (levelName, platforms, playerSpawn, width, height);
}