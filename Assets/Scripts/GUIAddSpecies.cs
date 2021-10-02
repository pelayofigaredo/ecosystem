using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIAddSpecies : MonoBehaviour
{
    public Species species;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public Button button;

    public void Initialice(Species species)
    {
        this.species = species;
        name.text = species.SpeciesName;
        description.text = species.Description;
        button.onClick.AddListener((delegate { GUIManager.instance.SelectSpeciesToSpawn(species); }));
    }
}
