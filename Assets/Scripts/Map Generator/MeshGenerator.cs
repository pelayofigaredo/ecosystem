using UnityEngine;
/**
 * Clase estática encargada de la generación de mallas 3d.
 */
public static class MeshGenerator 
{
    //Devuelve un objeto del tipo MeshData, conteniendo la información geométrica de una malla creada en función de los parámetros dados
    public static MeshData GenerateMesh(float[,] altitudes, float altitudeScale, AnimationCurve _altitudeCurve, int lod)
    {
        AnimationCurve altitudeCurve = new AnimationCurve(_altitudeCurve.keys);
        int width = altitudes.GetLength(0);
        int height = altitudes.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int detailSimplification = lod * 2;
        if(detailSimplification == 0)
        {
            detailSimplification = 1;
        }
        int vertexPerLine = (width - 1) / detailSimplification + 1;

        MeshData meshData = new MeshData(vertexPerLine, vertexPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < height; y += detailSimplification)
        {
            for (int x = 0; x < width; x += detailSimplification)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, altitudeCurve.Evaluate(altitudes[x, y]) * altitudeScale, topLeftZ - y);
                meshData.uv[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                if(x < width -1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex,vertexIndex+ vertexPerLine + 1,vertexIndex+ vertexPerLine);
                    meshData.AddTriangle(vertexIndex + vertexPerLine
                        + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }

}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;

    private int tIndex = 0;

    //Constructor
    public MeshData(int width, int height)
    {
        vertices = new Vector3[width * height];
        uv = new Vector2[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
        tIndex = 0;
    }

    //Añade un nuevo triángulo
    public void AddTriangle(int a,int b, int c)
    {
        triangles[tIndex] = a;
        triangles[tIndex+1] = b;
        triangles[tIndex+2] = c;
        tIndex += 3;
    }

    //Genera una malla con la información contenida. 
    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        return mesh;
    }
}