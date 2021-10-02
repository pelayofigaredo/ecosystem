using UnityEngine;
using System.Collections.Generic;
/**
 * ScriptableObject que almacena los datos de una distribución de objetos en el mapa, incluido el Prefab a instanciar, la cantidad y el tipo de distribución. 
 */
[CreateAssetMenu(fileName = "ItemDistribution", menuName = "ScriptableObjects/Item Distribution", order = 1)]
public class ItemDistribution : ScriptableObject
{
    const float MAX_DISTANCE = 5000;

    public GameObject item;
    bool randomRotation;
    public Vector3 rotaion;
    public int number;
    public Vector2 maxDistance = Vector2.zero;
    private int currentNumber;
    public LayerMask placementMask;
    public int allowedFailedAttempts;
    int currentAllowedFailedAttempts;


    const float heightRayCast = 50;

    public int CurrentNumber { get => currentNumber; set => currentNumber = value; }

    public Transform[] Generate(Vector3 minCoord, Vector3 maxCoord)
    {
        List<Transform> transformList = new List<Transform>();
        CurrentNumber = number;
        randomRotation = !rotaion.Equals(Vector3.zero);
        currentAllowedFailedAttempts = allowedFailedAttempts;
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
                transformList.Add(Spawn(placement, item));
                CurrentNumber--;
            }
            else
            {
                currentAllowedFailedAttempts--;
            }
        }

        return transformList.ToArray();
    }


    //Dadas las coordenadas en X y Z, devuelve un vector con la posición del terreno en esas coordenadas.
    private Vector3 TryToGetPlacementPointAt(float x, float z)
    {
        RaycastHit hit;
        Vector3 position = new Vector3(x, heightRayCast, z);
        if (Physics.Raycast(position, Vector3.down, out hit, MAX_DISTANCE, placementMask))
        {
            
          //  if ((placementMask.value & (1 << hit.collider.gameObject.layer)) > 0)
           // {
                return hit.point;
          //  }
        }
        return new Vector3(0,float.MinValue,0); //Used to imply null
    }
    //Instancia el objeto dado en el punto dado, con una rotación dependiente de los parámetros de la distribución.
    public Transform Spawn(Vector3 pos, GameObject item)
    {
        if (!randomRotation)
        {
            return Instantiate(item, pos, Quaternion.identity).transform;
        }
        else
        {
            return Instantiate(item, pos, Quaternion.Euler(
                Random.Range(-rotaion.x, rotaion.x), 
                Random.Range(-rotaion.y, rotaion.y), 
                Random.Range(-rotaion.z, rotaion.z))).transform;
        }
    }



}
