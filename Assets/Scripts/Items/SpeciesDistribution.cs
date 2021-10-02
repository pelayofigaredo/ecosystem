using UnityEngine;

[CreateAssetMenu(fileName = "SpeciesDistribution", menuName = "ScriptableObjects/Species Distribution", order = 3)]

public class SpeciesDistribution : ScriptableObject
{
    const float MAX_DISTANCE = 500;

    public Species species;
    public int number;
    public Vector2 maxDistance = Vector2.zero;
    private int currentNumber;
    public LayerMask placementMask;
    public int allowedFailedAttempts;
    int currentAllowedFailedAttempts;


    const float heightRayCast = 100;

    public int CurrentNumber { get => currentNumber; set => currentNumber = value; }


    //Efectúa un switch en función del tipo de distribución y llama al método indicado. 
    public void Generate(Vector3 minCoord, Vector3 maxCoord)
    {
        CurrentNumber = number;
        currentAllowedFailedAttempts = allowedFailedAttempts;
                GenerateAtRandom(minCoord, maxCoord);
    }

    //Efectúa la distribución eligiendo puntos aleatorios del mapa
    void GenerateAtRandom(Vector3 minCoord, Vector3 maxCoord)
    {
        if (!maxDistance.Equals(Vector2.zero))
        {
            if (minCoord.x < maxDistance.x * -1)
            {
                minCoord.x = maxDistance.x * -1;
            }
            if (maxCoord.x > maxDistance.x)
            {
                maxCoord.x = maxDistance.x;
            }
            if (minCoord.z < maxDistance.y * -1)
            {
                minCoord.z = maxDistance.y * -1;
            }
            if (maxCoord.z > maxDistance.y)
            {
                maxCoord.z = maxDistance.y;
            }
        }
        float xRange = maxCoord.x - minCoord.x;
        float zRange = maxCoord.z - minCoord.z;

        while (currentAllowedFailedAttempts > 0 && CurrentNumber > 0)
        {
            float x = Random.Range(minCoord.x, minCoord.x + xRange);
            float z = Random.Range(minCoord.z, minCoord.z + zRange);

            Vector3 placement = TryToGetPlacementPointAt(x, z);
            if (placement != new Vector3(0, float.MinValue, 0))
            {
                Transform t = Spawn(placement, species.GeneratePrefab());
                t.name = species.SpeciesName + " - " + species.NumberOfMembers;
                Unit spawnUnit = t.GetComponent<Unit>();
                spawnUnit.unitRenderer.material.color = species.GetRandomColor();
                CurrentNumber--;
            }
            else
            {
                currentAllowedFailedAttempts--;
            }
        }
    }

    //Dadas las coordenadas en X y Z, devuelve un vector con la posición del terreno en esas coordenadas.
    private Vector3 TryToGetPlacementPointAt(float x, float z)
    {
        RaycastHit hit;
        Vector3 position = new Vector3(x, heightRayCast, z);
        if (Physics.Raycast(position, Vector3.down, out hit, MAX_DISTANCE, placementMask))
        {

                return hit.point;

        }
        return new Vector3(0, float.MinValue, 0); //Used to imply null
    }
    //Instancia el objeto dado en el punto dado, con una rotación dependiente de los parámetros de la distribución.
    public Transform Spawn(Vector3 pos,Transform item)
    {
           return Instantiate(item, pos, Quaternion.identity);
    }
}