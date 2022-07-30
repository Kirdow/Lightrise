using System;
using UnityEngine;

public struct CameraBound
{
    public float min;
    public float max;

    public int SafeMinTile => Mathf.FloorToInt(min - 1.0f);
    public int SafeMaxTile => Mathf.CeilToInt(max + 1.0f);

    public CameraBound(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}