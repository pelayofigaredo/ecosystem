using UnityEngine;

[CreateAssetMenu(fileName = "New Species", menuName = "ScriptableObjects/Species", order = 2)]
public class Species : ScriptableObject
{
    [SerializeField]
    string speciesName;
    [SerializeField]
    [TextArea()]
    string description;
    [SerializeField]
    Transform speciesPrefab;

    [SerializeField]
    Gradient speciesColor;

    int numberOfMembers;
    int aliveMembers;

    public string SpeciesName { get => speciesName;}
    public Transform SpeciesPrefab { get => speciesPrefab;}
    public int NumberOfMembers { get => numberOfMembers;}
    public string Description { get => description; set => description = value; }

    private void OnValidate()
    {
        numberOfMembers = 0;
        aliveMembers = 0;
    }

    public Transform GeneratePrefab() 
    {
        numberOfMembers++;
        aliveMembers++;
        return speciesPrefab;
    }

    public Color GetRandomColor()
    {
        return speciesColor.Evaluate(Random.value);
    }

    public int GetAliveMembers()
    {
        return aliveMembers;
    }

    public void DeathMember()
    {
        aliveMembers--;
    }
}
