using UnityEngine;

/// <summary>
/// Clase que en el inicio asigna un material aleatorio de la lista
/// </summary>
public class RandomMaterial : MonoBehaviour
{
    public Material[] materials;
    public Renderer rend;

    void Start()
    {
        rend.material = materials[Random.Range(0, materials.Length - 1)];
        Destroy(this);
    }

}
