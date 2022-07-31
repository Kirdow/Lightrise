using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using UnityEngine;

public class LevelLoader
{
    public static LevelData LoadLevel(string levelName)
    {
        var levelTexture = LoadLevelImage(levelName);
        if (levelTexture == null)
        {
            Debug.LogError($"Failed to load level texture: ({levelName})");
            return null;
        }

        ImageData imageData = new ImageData(levelTexture);

        List<PlatformSpawn> platforms = new List<PlatformSpawn>();
        PlayerSpawn player = null;
        LightSpawn light = null;
        int jumps = -1;
        int nextLevel = -1;

        for (int y = 0; y < imageData.height; y++)
        {
            for (int x = 0; x < imageData.width; x++)
            {
                if (!imageData[x, y]) continue;

                if (y > 0)
                {
                    if (imageData.GetRed(x, y) == 255)
                    {
                        if (player == null) player = new PlayerSpawn(x, y);
                    }
                    else if (imageData.GetRed(x, y) == 128)
                    {
                        if (light == null) light = new LightSpawn(x, y);
                    }
                    else if (imageData.GetRed(x, y) == 0 && imageData.GetGreen(x, y) == 0)
                    {
                        int blue = imageData.GetBlue(x, y);
                        int platformSize = blue - 200;
                        if (platformSize > 0) platforms.Add(new PlatformSpawn(x, y, platformSize));
                    }
                }
                else
                {
                    int red = imageData.GetRed(x, y);
                    int green = imageData.GetGreen(x, y);
                    int blue = imageData.GetBlue(x, y);

                    switch (x)
                    {
                        case 0:
                            if (green >= 100)
                                jumps = green - 100;
                            break;
                        case 1:
                            if (green >= 100)
                                nextLevel = green - 100;
                            break;
                    }
                }
            }
        }

        if (player == null || platforms.Count == 0 || light == null || jumps <= 0)
        {
            Debug.LogError($"Failed to recognize level: ({levelName})");
            if (player == null)
                Debug.LogError("Player spawn not found");
            if (platforms.Count == 0)
                Debug.LogError("No platforms found");
            if (light == null)
                Debug.LogError("No light/goal found");
            if (jumps <= 0)
                Debug.LogError("Jumps not set");
            return null;
        }
        return new LevelData(levelName, platforms.ToArray(), player, light, imageData.width, imageData.height, jumps, nextLevel);
    }


    // Internal shit

    private static Regex LevelNameRegex = new Regex(@"^[A-Za-z]+_[0-9]+$");

    public static bool IsValidLevelName(string levelName) => LevelNameRegex.IsMatch(levelName);
    public static bool IsValidExternalLevel(string levelName)
    {
        string assetPath = GetLevelPath(levelName, true);
        if (assetPath == null) return false;

        return File.Exists(assetPath);
    }

    private static Texture2D LoadLevelInternally(string levelName)
    {
        string assetPath = GetLevelPath(levelName);
        if (assetPath == null) return null;
        return Resources.Load<Texture2D>(assetPath);
    }

    private static Texture2D LoadLevelExternally(string levelName)
    {
        string assetPath = GetLevelPath(levelName, true);
        if (assetPath == null) return null;

        Texture2D texture = null;

        if (File.Exists(assetPath))
        {
            try
            {
                var fileData = File.ReadAllBytes(assetPath);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
            }
            catch (Exception ignored) { }
        }
        else
        {
            Debug.LogWarning($"Level file not found: {assetPath}");
            Debug.LogWarning("Make sure to place them as 'Levels/L_<level name>.png'");
        }

        return texture;
    }

    private static Texture2D LoadLevelImage(string levelName)
    {
        Texture2D texture;

        texture = LoadLevelInternally(levelName);
        if (texture != null) return texture;

        Debug.LogWarning($"Level Loaded Externally: ({GetLevelPath(levelName, true)})");
        return LoadLevelExternally(levelName);
    }

    private static string GetLevelPath(string levelName, bool external = false)
    {
        if (!IsValidLevelName(levelName)) return null;

        return $"Levels/L_{levelName}{(external ? ".png" : "")}";
    }
}