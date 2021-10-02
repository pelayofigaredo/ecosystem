using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * Clase principal de gestión de la partida, entre otras cosas gestiona la creación del mapa y la distribución de elementos en este, así como la creación de la malla de navegación.
 */

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    GUIManager gUIManager;
    public LocalNavMeshBuilder navMeshBuilder;

    public TerrainGenerator terrain;
    public bool randomSeed = true;
    Vector3 terrainSize;


    public LayerMask placementMask;
    private float heightRayCast = 200;
    private float MAX_DISTANCE = 500;

    public SpeciesDistribution[] initialSpecies;

    [Header("Vegetation")]
    public ItemDistribution vegetationDistribution;
    public Transform vegetationParent;
    Transform[] vegetationList;

    const float borderMargin = 250;

    public Transform foodPrefab;

    private float fixedDeltaTime;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start()
    {
        gUIManager = GUIManager.instance;

        if (randomSeed)
        {
            terrain.RandomiceSeed();
        }

        terrain.DisplayOnGame();

        terrainSize = terrain.RealSize();
       // navMeshBuilder.m_Size = terrainSize;
        navMeshBuilder.UpdateNavMesh(false);

        Vector3 minPos = new Vector3(-terrainSize.x + borderMargin, 0, -terrainSize.z + borderMargin);
        Vector3 maxPos = new Vector3(terrainSize.x - borderMargin, 0, terrainSize.z - borderMargin);
        vegetationList = vegetationDistribution.Generate(minPos, maxPos);
        foreach(Transform t in vegetationList)
        {
            t.parent = vegetationParent;
        }
        if(initialSpecies.Length > 0)
        {
            foreach (SpeciesDistribution sd in initialSpecies)
            {
                sd.Generate(minPos, maxPos);
            }
        }
    }

    public void RandomizeMap()
    {
        terrain.RandomiceSeed();
        terrain.DisplayOnGame();
        navMeshBuilder.UpdateNavMesh(false);
        OnRandomizeMap();
        foreach (Transform t in vegetationList)
        {
            t.position = FindPosition(t.position.x,t.position.z);
        }
    }

    public System.Action OnRandomizeMap;

    public Vector3 FindPosition(float x, float z)
    {
        RaycastHit hit;
        Vector3 position = new Vector3(x, heightRayCast, z);
        if (Physics.Raycast(position, Vector3.down, out hit, MAX_DISTANCE, placementMask))
        {
                return hit.point;
        }
        return new Vector3(0, float.MinValue, 0);
    }


    //Cierra la aplicación.
    public void Exit()
    {
        Application.Quit();
    }


    internal void SetTime(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }


}
