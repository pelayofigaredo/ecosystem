using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Food))]
/**
 * Clase principal de los personajes. Almacena información como la distacia de escucha o la estamina. Requiere un nav  
 * mesh agent al que el arbol de decisiones accede para efectuar movimientos.
 */

public class Unit : MonoBehaviour
{
    const float HUNGER_DAMAGE_RATE =2;
    const float OLD_AGE_DAMAGE_RATE = 4;

    NavMeshAgent agent;
    [SerializeField]
    public Renderer unitRenderer;
    public Transform speciesGO;

    public enum TargetType { Prey, Danger, Mate }
    private Transform target;
    private Transform dangerTarget;
    private Transform mateTarget;

    public bool isAlive = true;

    public bool isBusy = false;
    [HideInInspector]
    public string actionName = "";


    //Species
    [Header("Species")]
    public string speciesName;
    public int generation = 0;


    public LayerMask predatorLayers;
    public LayerMask preyLayers;
    public LayerMask mateLayers;

    //Resources
    [Header("Speed")]
    [SerializeField]
    float speed;
    [Header("Health")]
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float health = 100;
    [Header("Stamina")]
    [SerializeField]
    private float maxStamina = 100;
    [SerializeField]
    private float stamina = 100;
    [SerializeField]
    private float staminaPercentThreshold = 0.3f;
    [SerializeField]
    private float restValue = 1;
    [Header("Nutrition")]
    [SerializeField]
    private float maxNutrition = 100;
    [SerializeField]
    private float nutrition = 100;
    [SerializeField]
    private float nutritionPercentThreshold = 0.5f;
    [SerializeField]
    private float hungerDecayRate = 0.5f;
    [Header("Reproduction")]
    [SerializeField]
    private float sexualUrgeThreshold = 100;
    [SerializeField]
    private float sexualUrgeIncrease = 0.1f;
    [SerializeField]
    float sexualUrge = 0;
    [SerializeField]
    private float reproductionStaminaPercentCost = 0.5f;
    [SerializeField]
    private float reproductionNutritionPercentCost = 0.5f;
    [Header("Age")]
    [SerializeField]
    private float maxAge = 200;
    [SerializeField]
    private float ageIncrease = 0.1f;
    float age;
    [Header("Senses")]
    private float detectionDistance = 10;
    [SerializeField]
    private float detectionDistanceBase = 10;

    [Header("Effects")]
    Material material;


    public Transform deathParticles;

    public NavMeshAgent Agent { get => agent;}
    public float DetectionDistance { get => detectionDistance;}
    
    public Transform Target { get => target; set => target = value; }
    public Transform DangerTarget { get => dangerTarget; set => dangerTarget = value; }
    public Transform MateTarget { get => mateTarget; set => mateTarget = value; }
    public float Speed { get => speed; set => speed = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MaxNutrition { get => maxNutrition; set => maxNutrition = value; }
    public float StaminaPercentThreshold { get => staminaPercentThreshold; set => staminaPercentThreshold = value; }
    public float RestValue { get => restValue; set => restValue = value; }
    public float NutritionPercentThreshold { get => nutritionPercentThreshold; set => nutritionPercentThreshold = value; }
    public float HungerDecayRate { get => hungerDecayRate; set => hungerDecayRate = value; }
    public float SexualUrgeIncrease { get => sexualUrgeIncrease; set => sexualUrgeIncrease = value; }
    public float DetectionDistanceBase { get => detectionDistanceBase; set => detectionDistanceBase = value; }
    public Material Material { get => material; set => material = value; }
    public float SexualUrge { get => sexualUrge; set => sexualUrge = value; }
    public float Nutrition { get => nutrition; set => nutrition = value; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float ReproductionNutritionPercentCost { get => reproductionNutritionPercentCost; set => reproductionNutritionPercentCost = value; }
    public float ReproductionStaminaPercentCost { get => reproductionStaminaPercentCost; set => reproductionStaminaPercentCost = value; }
    public float Health { get => health; set => health = value; }
    public float MaxAge { get => maxAge; set => maxAge = value; }
    public float AgeIncrease { get => ageIncrease; set => ageIncrease = value; }
    public float Age { get => age; set => age = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<Food>().OnBeenEaten += Die;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = speed;
        sexualUrge = 0;
        stamina = maxStamina;
        nutrition = maxNutrition;
        health = maxHealth;
        detectionDistance = detectionDistanceBase;
        age = 0;
        if (unitRenderer == null)
        {
            unitRenderer = GetComponentInChildren<Renderer>();
        }
        material = unitRenderer.material;
        GameManager.instance.OnRandomizeMap += OnRandomizeMap;
    }

    internal void SetStateName(string stateName)
    {
        actionName = stateName;
    }

    private void Update()
    {
        nutrition -= hungerDecayRate;
        if(nutrition < 0)
        {
            nutrition = 0;
            health -= Time.deltaTime * HUNGER_DAMAGE_RATE;
        }
        sexualUrge += sexualUrgeIncrease * Time.deltaTime;
        age += ageIncrease;
        if (age > maxAge)
        {
            health -= Time.deltaTime * OLD_AGE_DAMAGE_RATE;
        }
        if(health <= 0)
        {
            Die();
        }
    }

    public void IncreaseStamina(float increase)
    {
        stamina += increase;
        if(stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }

    public void DecreaseStamina(ActionUnit action)
    {
        DecreaseStamina(action.staminaCost * Time.deltaTime);
    }

    public void DecreaseStamina(float decrease)
    {
        stamina -= decrease;
        if(stamina < 0)
        {
            stamina = 0;
        }
    }

    public void ModifySenses(float percent)
    {
        detectionDistance = detectionDistanceBase * percent;
    }

    public float GetHungerPercent()
    {
        return (nutrition / maxNutrition);
    }

    public float GetStaminaPercent()
    {
        return (stamina / maxStamina);
    }

    internal float GetHealthPercent()
    {
        return (health / maxHealth);
    }

    public float GetSexuaUrgePercent()
    {
        return (sexualUrge / sexualUrgeThreshold);
    }

    public float GetAgePercent()
    {
        return (age / maxAge);
    }

    public bool CanReproduce()
    {
        return (sexualUrge >= sexualUrgeThreshold) ? true : false;
    }

    public bool IsHungry()
    {
        bool isHungry = (GetHungerPercent() < nutritionPercentThreshold) ? true : false;
        return (GetHungerPercent() < nutritionPercentThreshold) ? true : false;
    }

    public bool IsTired()
    {
        return (GetStaminaPercent() < staminaPercentThreshold) ? true : false;
    }

    public void Rest()
    {
        stamina += Time.deltaTime * restValue;
    }

    public void Eat(Food food)
    {
        nutrition += food.NutritionalValue;
        if(nutrition > maxNutrition)
        {
            nutrition = maxNutrition;
        }
        food.Eated();
    }

    public void ApplyReproductionCost()
    {
        stamina -= (maxStamina * reproductionStaminaPercentCost);
        nutrition -= (maxNutrition * reproductionNutritionPercentCost);
        if(nutrition < 0)
        {
            nutrition = 0;
        }
    }

    public void TargetNearestPrey()
    {
        Transform nearest = null;
        float distance = float.MaxValue;
        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionDistance, preyLayers);
        if (hitColliders.Length > 0)
        {
            for(int i = 0; i < hitColliders.Length; i++)
            {
                float currentDistance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if(currentDistance < distance)
                {
                    distance = currentDistance;
                    nearest = hitColliders[i].transform;
                }
            }      
        }
        if(nearest != null)
        {
            target = nearest;
        }
    }

    public void TargetNearestPredator()
    {
        Transform nearest = null;
        float distance = float.MaxValue;
        if (dangerTarget != null)
        {
            distance = Vector3.Distance(transform.position, dangerTarget.position);
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionDistance, predatorLayers);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                float currentDistance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    nearest = hitColliders[i].transform;
                }
            }
        }
        if (nearest != null)
        {
            dangerTarget = nearest;
        }
    }

    public void TargetNearestMate()
    {
        Transform nearest = null;
        float distance = float.MaxValue;
        if (mateTarget != null)
        {
            distance = Vector3.Distance(transform.position, mateTarget.position);
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionDistance, mateLayers);
        if (hitColliders.Length > 1)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (!hitColliders[i].gameObject.Equals(gameObject))
                {
                    float currentDistance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        nearest = hitColliders[i].transform;
                    }
                }

            }
        }
        if (nearest != null)
        {
            mateTarget = nearest;
        }
    }

    public void SetValues(float speed, float maxStamina, float maxHealth,
            float maxNutrition,  float staminaPercentThreshold, float restValue,
            float nutritionPercentThreshold,  float hungerDecayRate, float sexualUrgeIncrease,
            float detectionDistanceBase, float reproductionStaminaPercentCost, float reproductionNutritionPercentCost, float maxAge, float ageIncrease)
    {
        this.speed = speed;

        this.maxStamina = maxStamina;
        this.maxHealth = maxHealth;
        this.maxNutrition = maxNutrition;
        this.staminaPercentThreshold = staminaPercentThreshold;
        this.restValue = restValue;
        this.nutritionPercentThreshold = nutritionPercentThreshold;
        this.hungerDecayRate = hungerDecayRate;
        this.sexualUrgeIncrease = sexualUrgeIncrease;
        this.detectionDistanceBase = detectionDistanceBase;
        this.reproductionStaminaPercentCost = reproductionStaminaPercentCost;
        this.reproductionNutritionPercentCost = reproductionNutritionPercentCost;
        this.maxAge = maxAge;
        this.ageIncrease = ageIncrease;

        agent.speed = speed;
        sexualUrge = 0;
        age = 0;
        stamina = maxStamina;
        nutrition = maxNutrition;
        health = maxHealth;
        detectionDistance = detectionDistanceBase;
        if (unitRenderer == null)
        {
            unitRenderer = GetComponentInChildren<MeshRenderer>();
        }
        material = unitRenderer.material;
    }

    void OnRandomizeMap()
    {
        transform.position = GameManager.instance.FindPosition(transform.position.x, transform.position.z);
    }

    public void Die()
    {
        if (isAlive)
        {
            GameManager.instance.OnRandomizeMap -= OnRandomizeMap;
            GUIManager gui = GUIManager.instance;
            if (gui.selectedUnit != null && gui.selectedUnit == this)
            {
                gui.DeselectUnit();
            }
            isAlive = false;
            unitRenderer.enabled = false;
            GetComponent<Collider>().enabled = false;
            isBusy = true;
            if (deathParticles != null)
            {
                Instantiate(deathParticles, transform.position, deathParticles.rotation);
            }
            State[] states = gameObject.GetComponentsInChildren<State>();
            foreach (State s in states)
            {
                s.enabled = false;
            }
            EvolutionManager.instance.UnitDeath(this);
            Destroy(gameObject, 0.5f);
        }

    }

}
