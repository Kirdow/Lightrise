using UnityEngine;

public class ImageData
{
    public int width, height;
    public int[] pixels;

    public ImageData(Texture2D image)
    {
        width = image.width;
        height = image.height;

        pixels = new int[width * height];
        
        Color32[] colors = image.GetPixels32();
        Color32 color;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                color = colors[x + y * width];
                pixels[x + y * width] =
                    (color.r << 16) |
                    (color.g << 8) |
                    (color.b << 0) |
                    (color.a << 24);
            }
        }
    }

    public int GetRed(int x, int y) => (pixels[x + y * width] >> 16) & 0xFF;
    public int GetGreen(int x, int y) => (pixels[x + y * width] >> 8) & 0xFF;
    public int GetBlue(int x, int y) => (pixels[x + y * width]) & 0xFF;

    public bool this[int x, int y] => (pixels[x + y * width] & 0xFFFFFF) != 0xFFFFFF;
}