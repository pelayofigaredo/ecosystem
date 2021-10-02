
using UnityEngine;

/**
 * Clase estática encargada de la generación de ruido
 */

public static class Noise 
{

    //Devuelve una matriz normalizada de valores obtenida de evaluar el ruido perlin en función de los parámetros dados.
    public static float[,] GenerateNoise (int size,float scale,int octaves, float persistence, float lacunarity, int seed, Vector2 offset)
    {
        float[,] noise = new float[size, size];

        System.Random rnd = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];
        float maxPossibleAltitude = 0;
        float amplitude = 1;
        float frequency = 1;


        for (int i = 0; i < octaves; i++)
        {
            float x = rnd.Next(-10000, 10000) + offset.x;
            float y = rnd.Next(-10000, 10000) - offset.y;
            octaveOffsets[i] = new Vector2(x, y);
            maxPossibleAltitude += amplitude;
            amplitude *= persistence;
        }

        if(scale <= 0)
        {
            scale = 0.00001f;
        }

        float maxLocal = float.MinValue;
        float minLocal = float.MaxValue;

        float halfWidth = size / 2;
        float halfHeight = size / 2;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {

                amplitude = 1;
                frequency = 1;
                float altitude = 0;

                for(int i = 0; i < octaves; i++) //Octaves loop
                {
                    float sampleX = (x-halfWidth + octaveOffsets[i].x) / scale * frequency ;
                    float sampleY = (y-halfHeight + octaveOffsets[i].y) / scale * frequency ;
                    float value = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    altitude += value * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if(altitude > maxLocal)
                {
                    maxLocal = altitude;
                }
                else if (altitude < minLocal)
                {
                    minLocal = altitude;
                }

                noise[x, y] = altitude;

            }
        }

        //Normalice
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                 noise[x, y] = Mathf.InverseLerp(minLocal, maxLocal, noise[x, y]);
            }
        }

                return noise;
    }
}
