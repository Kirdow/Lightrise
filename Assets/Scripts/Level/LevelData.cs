using UnityEngine;

public class LevelData
{
    public string LevelName { get; private set; }

    public PlatformSpawn[] PlatformSpawnLocations { get; private set; }
    public PlayerSpawn PlayerSpawnLocation { get; private set; }
    public LightSpawn LightSpawnLocation { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Jumps { get; private set; }
    public int NextLevel { get; private set; }

    public LevelData(string levelName, PlatformSpawn[] platforms, PlayerSpawn playerSpawn, LightSpawn lightSpawn, int width, int height, int jumps, int nextLevel)
        => (LevelName,  PlatformSpawnLocations, PlayerSpawnLocation,    LightSpawnLocation,    Width, Height, Jumps, NextLevel)
        =  (levelName,  platforms,              playerSpawn,            lightSpawn,            width, height, jumps, nextLevel);
}