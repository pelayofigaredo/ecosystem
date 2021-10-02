using UnityEngine;

/**
 * Se encarga de aplicar la información del mapa en una malla o una textura
 */

public class MapDisplay : MonoBehaviour
{
    public Renderer target;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;

    //Dibuja la textura dada en el Renderer almacenado como target.
    public void Draw(Texture2D texture)
    {
        target.sharedMaterial.SetTexture("_MainTexture",texture);
        target.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    //Aplica la malla generada por el MeshData dado, y posteriormente le aplica también la textura a su shader.
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        Mesh mesh = meshData.GetMesh();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.sharedMaterial.SetTexture("_MainTexture",texture);
    }
}
