using UnityEngine;

public class LevelData
{
    public string LevelName { get; private set; }

    public PlatformSpawn[] PlatformSpawnLocations { get; private set; }
    public PlayerSpawn PlayerSpawnLocation { get; private set; }

    public LevelData(string levelName, PlatformSpawn[] platforms, PlayerSpawn playerSpawn)
        => (LevelName, PlatformSpawnLocations, PlayerSpawnLocation) = (levelName, platforms, playerSpawn);
}