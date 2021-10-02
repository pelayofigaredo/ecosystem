using System;
using UnityEngine;

/**
 * Clase bastante compleja, que se encarga de la generación del terreno mediante el empleo de Noise, MeshGenerator y TextureGenerator. 
 * Sus parámetros pueden ser altamente modificados en el inspector para obtener distintos estilos de terreno 
 */

public class TerrainGenerator : MonoBehaviour
{
    public enum DrawMode {Noise,Altitude,Mesh,FallOff};
    public DrawMode drawMode;

    [Range(40, 256)]
    public int size = 241;
    [Range(0,6)]
    public int lod;

    [Range(5, 20)]
    public float altitudeScale;
    public AnimationCurve altitudeCurve;
    [Range(7, 70)]
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public int lacunarity;
    public bool useFalloff;
    public Vector2 offset;
    public int seed;

    public bool update;

    public AltitudeType[] altitudeRegions;

    float[,] fallOffMap;
    MapDisplay mapDisplay;


    private void Awake()
    {
        mapDisplay = GetComponent<MapDisplay>();
        if (useFalloff)
        {
            fallOffMap = FalloffGenerator.GenerateFallOffMap(size);
        }
    }

    //Emplea la configuración actual para generar un mapa de ruido, que contiene los valores de altura y los colores de cada pixel en función de esta.
    public MapData GenerateNoiseMap()
    {
        
        float[,] currentMap = Noise.GenerateNoise(size, noiseScale, octaves, persistence, lacunarity, seed, offset);
        Color[] altitudeColors = GetAltitudeFromNoise(currentMap);
        return new MapData(currentMap, altitudeColors);
    }
    //Genera y muestra el mapa de la forma seleccionada, bien en malla o bien en una textura renderizada en un plano.
    public void DisplayOnEditor()
    {
        MapData mapData = GenerateNoiseMap();
        switch (drawMode)
        {
            case DrawMode.Noise:
                mapDisplay.Draw(TextureGenerator.GetTextureFromFloat(mapData.altitudes, size, size));
                break;

            case DrawMode.Altitude:
                mapDisplay.Draw(TextureGenerator.GetTextureFromColors(mapData.colors, size, size));
                break;

            case DrawMode.Mesh:
                mapDisplay.DrawMesh(MeshGenerator.GenerateMesh(mapData.altitudes, altitudeScale, altitudeCurve, lod), TextureGenerator.GetTextureFromColors(mapData.colors, size, size));
                break;

            case DrawMode.FallOff:
                mapDisplay.Draw(TextureGenerator.GetTextureFromFloat(FalloffGenerator.GenerateFallOffMap(size),size,size));
                break;
        }
    }

    //Similar al método anterior pero fuerza que la generación se haga en una malla para ser usada en la partida.
    public void DisplayOnGame()
    {
        MapData mapData = GenerateNoiseMap();
        mapDisplay.DrawMesh(MeshGenerator.GenerateMesh(mapData.altitudes, altitudeScale, altitudeCurve, lod), TextureGenerator.GetTextureFromColors(mapData.colors, size, size));
    }

    //Devuelve un array de colores derivado del recibido en función de las diferentes regiones de altura.
    private Color[] GetAltitudeFromNoise(float[,] currentMap)
    {
        Color[] colors = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (useFalloff)
                {
                    currentMap[x, y] = Mathf.Clamp(currentMap[x, y] - fallOffMap[x, y],0,1);
                }
                float currentAltitude = currentMap[x, y];
                colors[y * size + x] = GetAltitudeRegion(currentAltitude).colour;
            }
        }
        return colors;
    }

    //Dado un valor devuelve la región de altura a la que pertenece.
    private AltitudeType GetAltitudeRegion(float value)
    {
        for (int i = 0; i < altitudeRegions.Length; i++)
        {
            if(value <= altitudeRegions[i].height)
            {
                return altitudeRegions[i];
            }
        }

        return altitudeRegions[altitudeRegions.Length-1];
    }

    private void OnValidate()
    {
        mapDisplay = GetComponent<MapDisplay>();
        fallOffMap = FalloffGenerator.GenerateFallOffMap(size);
      
    }

    //Cambia la semilla a un valor aleatorio
    public void RandomiceSeed()
    {
        seed = Mathf.RoundToInt(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
    }

    //Devuelve el tamaño real en unidades del terreno
    public Vector3 RealSize()
    {
        Bounds bounds = mapDisplay.meshCollider.bounds;
        return bounds.size;
    }
    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T paramenter;

        public MapThreadInfo(Action<T> callback, T paramenter)
        {
            this.callback = callback;
            this.paramenter = paramenter;
        }
    }
}
[System.Serializable]
public struct AltitudeType
{
    public string name;
    public float height;
    public Color colour;
}
public struct MapData
{
    public readonly float[,] altitudes;
    public readonly Color[] colors;

    public MapData(float[,] altitudes, Color[] colors)
    {
        this.altitudes = altitudes;
        this.colors = colors;
    }
}
