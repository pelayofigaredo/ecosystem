using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

/**
 * Clase que gestiona la interfaz de usuario, permitiendo elegir la cantidad de informacion mostrada, alterar la velocidad o reinicira la simulación
 * */
public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;
    GameManager gameManager;
    EvolutionManager evolutionManager;

    [Header("Species info")]
    public GameObject speciesPannel;
    public TextMeshProUGUI speciesTMP;
    StringBuilder sb = new StringBuilder();

    [Header("Unit Selection")]
    public GameObject unitPannel;
    public LineRenderer unitPathRenderer;
    public Transform unitDetectionSphere;

    public TextMeshProUGUI unitNameTMP;
    public TextMeshProUGUI unitGenerationTMP;
    public TextMeshProUGUI unitActionTMP;
    public TextMeshProUGUI unitSpeedTMP;
    public TextMeshProUGUI unitAgeTMP;
    public Slider unitAgeSlider;
    public TextMeshProUGUI unitHealthTMP;
    public Slider unitHealthSlider;
    public TextMeshProUGUI unitStaminaTMP;
    public Slider unitStaminaSlider;
    public TextMeshProUGUI unitNutritionTMP;
    public Slider unitNutritionSlider;
    public TextMeshProUGUI unitSexualUrgeTMP;
    public Slider unitSexualUrgeSlider;

    [HideInInspector]
    public Unit selectedUnit;
    bool hasUnitSelected;

    

    [Header("Options")]
    public Slider timeSlider;
    public TextMeshProUGUI timeTMP;
    public Button randomizeMapButton;
    public Button spawnFoodButton;
    public Button spawnUnitButton;
    bool isPlacingFood = false;
    bool isPlacingUnit = false;
    Species speciesToSpawn;

    [Header("Unit Spawn")]
    public GameObject unitSpawnPannel;
    public GameObject addSpeciesButtonPrefab;

    private bool playing = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        gameManager = GameManager.instance;
        evolutionManager = EvolutionManager.instance;

        timeSlider.onValueChanged.AddListener(delegate { ChangeTime(); });
        randomizeMapButton.onClick.AddListener(delegate { RandomizeMap(); });
        spawnFoodButton.onClick.AddListener(delegate { OnSpawFoodToggle(); });
        spawnUnitButton.onClick.AddListener(delegate { EnableunitSpawnPannel(!unitSpawnPannel.activeSelf); });

        foreach(Species s in evolutionManager.speciesArray)
        {
            GameObject go = Instantiate(addSpeciesButtonPrefab, unitSpawnPannel.transform);
            GUIAddSpecies gas = go.GetComponent<GUIAddSpecies>();
            gas.Initialice(s);
        }
        EnableunitSpawnPannel(false);
        DeselectUnit();
    }

    private void Update()
    {
        UpdateSpeciesInfo();
        if (hasUnitSelected)
        {
            UpdateUnitValues();
        }
        if (Input.GetMouseButton(1))
        {
            isPlacingFood = false;
            isPlacingUnit = false;
        }
            if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (isPlacingFood)
            {
                    Vector3 pos = hit.point;
                    pos.y += 1;
                    Instantiate(gameManager.foodPrefab, pos, Quaternion.identity);
            }
            else if (isPlacingUnit)
            {
                    Transform t = Instantiate(speciesToSpawn.GeneratePrefab(), hit.point, Quaternion.identity);
                    t.name = speciesToSpawn.SpeciesName + " - " + speciesToSpawn.NumberOfMembers;
                    Unit spawnUnit = t.GetComponent<Unit>();
                    spawnUnit.unitRenderer.material.color = speciesToSpawn.GetRandomColor();
                }
            else
            {
                    Unit u = hit.transform.GetComponent<Unit>();
                    if (u != null)
                    {
                        SelectUnit(u);
                    }
                    else
                    {
                        DeselectUnit();
                    }
                }       
            }
        }
    }

    public void ChangeTime()
    {
        float value = (float)System.Math.Round((double)timeSlider.value,2);
        gameManager.SetTime(value);
        timeTMP.text = value.ToString();
    }

    public void RandomizeMap()
    {
        gameManager.RandomizeMap();
    }

    void OnSpawFoodToggle()
    {
        isPlacingFood = true;
        isPlacingUnit = false;
    }

    void EnableunitSpawnPannel(bool enable)
    {
        unitSpawnPannel.SetActive(enable);
    }

    public void SelectSpeciesToSpawn(Species species)
    {
        speciesToSpawn = species;
        isPlacingFood = false;
        isPlacingUnit = true;
    }

    void UpdateSpeciesInfo()
    {
        sb.Clear();
        for(int i = 0; i < evolutionManager.speciesArray.Length; i++)
        {
            Species s = evolutionManager.speciesArray[i];
            sb.Append(s.name);
            sb.Append(": ");
            sb.Append(s.GetAliveMembers());
            sb.AppendLine();
        }
        speciesTMP.text = sb.ToString();
    }

    void SelectUnit(Unit unit)
    {
        unitPannel.SetActive(true);
        selectedUnit = unit;
        hasUnitSelected = true;
        unitNameTMP.text = "Name: " + unit.gameObject.name;
        unitGenerationTMP.text = "Generation: " + unit.generation;
        unitPathRenderer.enabled = true;
        unitSpeedTMP.text = System.Math.Round(selectedUnit.Speed, 2).ToString();
        unitDetectionSphere.gameObject.SetActive(true);
        UpdateUnitValues();
    }

    void UpdateUnitValues()
    {
        unitActionTMP.text = "Action: " + selectedUnit.actionName;
        unitAgeTMP.text = System.Math.Round(selectedUnit.Age, 2).ToString();
        unitAgeSlider.value = selectedUnit.GetAgePercent();
        unitHealthTMP.text = System.Math.Round(selectedUnit.Health, 2).ToString();
        unitHealthSlider.value = selectedUnit.GetHealthPercent();
        unitStaminaTMP.text = System.Math.Round(selectedUnit.Stamina, 2).ToString();
        unitStaminaSlider.value = selectedUnit.GetStaminaPercent();
        unitNutritionTMP.text = System.Math.Round(selectedUnit.Nutrition, 2).ToString();
        unitNutritionSlider.value = selectedUnit.GetHungerPercent();
        unitSexualUrgeTMP.text = System.Math.Round(selectedUnit.SexualUrge, 2).ToString();
        unitSexualUrgeSlider.value = selectedUnit.GetSexuaUrgePercent();
        unitDetectionSphere.transform.position = selectedUnit.transform.position;
        unitDetectionSphere.transform.localScale = Vector3.one * 2 * selectedUnit.DetectionDistance;
        unitPathRenderer.positionCount = selectedUnit.Agent.path.corners.Length;
        unitPathRenderer.SetPositions(selectedUnit.Agent.path.corners);
    }

    public void DeselectUnit()
    {
        unitPannel.SetActive(false);
        hasUnitSelected = false;
        unitPathRenderer.enabled = false;
        unitDetectionSphere.gameObject.SetActive(false);
    }
}
