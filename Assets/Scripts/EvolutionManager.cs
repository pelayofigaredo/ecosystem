using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    public EvolutionConfiguration configuration;
    public static EvolutionManager instance;

    public int unitLimit = 500;

    public Species[] speciesArray;
    Dictionary<string, Species> species = new Dictionary<string, Species>();

    GameManager gameManager;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }


        for (int i = 0; i < speciesArray.Length; i++)
        {
            species.Add(speciesArray[i].name, speciesArray[i]);
        }
    }

    void Start()
    {
        gameManager = GameManager.instance;

    }

    public void Reproduce(Unit mainUnit, Unit secondUnit)
    {
        if (CanSpawn())
        {
            //Apply reproduction costs
            mainUnit.ApplyReproductionCost();
            secondUnit.ApplyReproductionCost();
            //Selecciona cada attributo en funcion de la contribucion del principal
            float mainConttribution = configuration.MainConttribution;
            float speed = (Random.value > mainConttribution) ? mainUnit.Speed : secondUnit.Speed;
            float maxStamina = (Random.value > mainConttribution) ? mainUnit.MaxStamina : secondUnit.MaxStamina;
            float maxHealth = (Random.value > mainConttribution) ? mainUnit.MaxHealth : secondUnit.MaxHealth;
            float maxNutrition = (Random.value > mainConttribution) ? mainUnit.MaxNutrition : secondUnit.MaxNutrition;
            float staminaPercentThreshold = (Random.value > mainConttribution) ? mainUnit.StaminaPercentThreshold : secondUnit.StaminaPercentThreshold;
            float restValue = (Random.value > mainConttribution) ? mainUnit.RestValue : secondUnit.RestValue;
            float nutritionPercentThreshold = (Random.value > mainConttribution) ? mainUnit.NutritionPercentThreshold : secondUnit.NutritionPercentThreshold;
            float hungerDecayRate = (Random.value > mainConttribution) ? mainUnit.HungerDecayRate : secondUnit.HungerDecayRate;
            float sexualUrgeIncrease = (Random.value > mainConttribution) ? mainUnit.SexualUrgeIncrease : secondUnit.SexualUrgeIncrease;
            float detectionDistanceBase = (Random.value > mainConttribution) ? mainUnit.DetectionDistanceBase : secondUnit.DetectionDistanceBase;
            float reproductionStaminaPercentCost = (Random.value > mainConttribution) ? mainUnit.ReproductionStaminaPercentCost : secondUnit.ReproductionStaminaPercentCost;
            float reproductionNutritionPercentCost = (Random.value > mainConttribution) ? mainUnit.ReproductionNutritionPercentCost : secondUnit.ReproductionNutritionPercentCost;
            float maxAge = (Random.value > mainConttribution) ? mainUnit.MaxAge : secondUnit.MaxAge;
            float ageIncrease = (Random.value > mainConttribution) ? mainUnit.AgeIncrease : secondUnit.AgeIncrease;

            //Se aplica la mutacion en función de la configuracion ft Darwin
            configuration.ApplyVariance(ref speed, ref maxStamina, ref maxHealth, ref maxNutrition, ref staminaPercentThreshold,
                ref restValue, ref nutritionPercentThreshold, ref hungerDecayRate, ref sexualUrgeIncrease, ref detectionDistanceBase,
                ref reproductionStaminaPercentCost, ref reproductionNutritionPercentCost, ref maxAge, ref ageIncrease);

            //Instancia el Game Object
            Species childSpecies = species[mainUnit.speciesName];
            Transform speciesTransform = childSpecies.GeneratePrefab();
            Transform child = Instantiate(speciesTransform, gameManager.FindPosition(mainUnit.transform.position.x, mainUnit.transform.position.y)
                , Quaternion.identity);
            child.name = mainUnit.speciesName + " - " + childSpecies.NumberOfMembers;
            Unit childUnit = child.GetComponent<Unit>();
            childUnit.SetValues(speed, maxStamina, maxHealth, maxNutrition, staminaPercentThreshold, restValue, nutritionPercentThreshold, hungerDecayRate, sexualUrgeIncrease,
                detectionDistanceBase, reproductionStaminaPercentCost, reproductionNutritionPercentCost, maxAge, ageIncrease);
            childUnit.generation = mainUnit.generation + 1;
            Color childColor = Color.Lerp(mainUnit.Material.color, secondUnit.Material.color, configuration.GetColorLerp());
            childUnit.Material.color = childColor;
            //Reduces main sexual urge
            mainUnit.SexualUrge = 0;
        }
    }

    public void UnitDeath(Unit unit)
    {
        species[unit.speciesName].DeathMember();
    }

    bool CanSpawn()
    {
        int current = 0;
        foreach(Species s in speciesArray)
        {
            current += s.GetAliveMembers();
        }

        return (current < unitLimit) ? true : false;
    }

    }
