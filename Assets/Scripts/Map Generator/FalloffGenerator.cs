using UnityEngine;

/**
 * Clase estática que genera un degradado dado por una ecuación.
 */

public class FalloffGenerator 
{
    // Devuelve el degradado en forma de matriz.
    public static float[,] GenerateFallOffMap(int size)
    {
        float[,] map = new float[size, size];

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    //Calcula la ecuación para el punto dado
    static float Evaluate(float f)
    {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(f, a) / (Mathf.Pow(f, a) + Mathf.Pow(b - b * f, a));
    }
}
