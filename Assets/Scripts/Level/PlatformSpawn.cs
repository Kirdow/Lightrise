using UnityEngine;

public class PlatformSpawn
{
    public int x, y;
    public int size;

    public PlatformSpawn(int x, int y, int size) => (this.x, this.y, this.size) = (x, y, size);
}