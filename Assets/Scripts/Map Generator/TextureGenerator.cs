using UnityEngine;


/**
 * Clase estática que genera texturas pixel a pixel.
 */

public static class TextureGenerator
{
    //Genera y devuelve una textura dadas las dimensiones de esta y un array de colores.
    public static Texture2D GetTextureFromColors(Color[] colors, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    //Genera y devuelve una textura dadas las dimensiones de esta y un array de float que el metodo interpretara como un degradado de blanco a negro.
    public static Texture2D GetTextureFromFloat(float[,] values, int width, int height)
    {
     
        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[y + width * x] = Color.Lerp(Color.black, Color.white, values[x, y]);
            }
        }

        return GetTextureFromColors(colors,width,height);
    }
}
