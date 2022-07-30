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

        for (int y = 0; y < imageData.height; y++)
        {
            for (int x = 0; x < imageData.width; x++)
            {
                if (!imageData[x, y]) continue;

                if (imageData.GetRed(x, y) == 255)
                {
                    if (player == null) player = new PlayerSpawn(x, y);
                }
                else if (imageData.GetRed(x, y) == 0 && imageData.GetGreen(x, y) == 0)
                {
                    int blue = imageData.GetBlue(x, y);
                    int platformSize = blue - 200;
                    if (platformSize > 0) platforms.Add(new PlatformSpawn(x, y, platformSize));
                }
            }
        }

        if (player == null || platforms.Count == 0)
        {
            Debug.LogError($"Failed to recognize level: ({levelName})");
            return null;
        }
        return new LevelData(levelName, platforms.ToArray(), player, imageData.width, imageData.height);
    }


    // Internal shit

    private static Regex LevelNameRegex = new Regex(@"^[A-Za-z][A-Za-z0-9_]*[A-Za-z0-9]$");

    private static bool IsValidLevelName(string levelName) => LevelNameRegex.IsMatch(levelName);

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