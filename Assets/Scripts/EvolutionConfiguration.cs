using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evolution Configuration", menuName = "ScriptableObjects/Evolution Configuration", order = 1)]
public class EvolutionConfiguration : ScriptableObject
{
    [Header("Mutation")]
    [SerializeField]
    [Range(0,1)]
    float mutationChance = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float mainConttribution = 0.5f;

    [Header("Mutation variance")]
    [SerializeField]
    float speedVariance;
    [SerializeField]
    float maxStaminaVariance;
    [SerializeField]
    float maxHealthVariance;
    [SerializeField]
    float maxNutritionVariance;
    [SerializeField]
    float staminaPercentThresholdVariance;
    [SerializeField]
    float restValueVariance;
    [SerializeField]
    float nutritionPercentThresholdVariance;
    [SerializeField]
    float hungerDecayRateVariance;
    [SerializeField]
    float sexualUrgeIncreaseVariance;
    [SerializeField]
    float detectionDistanceBaseVariance;
    [SerializeField]
    private float reproductionStaminaPercentCostVariance;
    [SerializeField]
    private float reproductionNutritionPercentCostVariance;
    [SerializeField]
    private float maxAgeVariance;
    [SerializeField]
    private float ageIncreaseVariance;

    [Header("Clamp values")]
    [SerializeField]
    bool clampValues;
    [SerializeField]
    Vector2 clampSpeed ;
    [SerializeField]
    Vector2 clampMaxStamina ;
    [SerializeField]
    Vector2 clampMaxHealth ;
    [SerializeField]
    Vector2 clampMaxNutrition ;
    [SerializeField]
    Vector2 clampStaminaPercentThreshold ;
    [SerializeField]
    Vector2 clampRestValue ;
    [SerializeField]
    Vector2 clampNutritionPercentThreshold ;
    [SerializeField]
    Vector2 clampHungerDecayRate ;
    [SerializeField]
    Vector2 clampSexualUrgeIncrease ;
    [SerializeField]
    Vector2 clampDetectionDistanceBase ;
    [SerializeField]
    Vector2 clampReproductionStaminaPercentCost;
    [SerializeField]
    Vector2 clampReproductionNutritionPercentCost;
    [SerializeField]
    Vector2 clampMaxAge;
    [SerializeField]
    Vector2 clampAgeIncrease;

    public float MainConttribution { get => 1 - mainConttribution; set => mainConttribution = value; }

    public void ApplyVariance(ref float speed, ref float maxStamina, ref float maxHealth, 
            ref float maxNutrition, ref float staminaPercentThreshold, ref float restValue, 
            ref float nutritionPercentThreshold, ref float hungerDecayRate, ref float sexualUrgeIncrease, 
            ref float detectionDistanceBase, ref float reproductionStaminaPercentCost, ref float reproductionNutritionPercentCost,
            ref float maxAge, ref float ageIncrease)
    {
        if(mutationChance == 1)
        {
            speed += Random.Range(-speedVariance, speedVariance);
            maxStamina += Random.Range(-maxStaminaVariance, maxStaminaVariance);
            maxHealth += Random.Range(-maxHealthVariance, maxHealthVariance);
            maxNutrition += Random.Range(-maxNutritionVariance, maxNutritionVariance);
            staminaPercentThreshold += Random.Range(-staminaPercentThresholdVariance, staminaPercentThresholdVariance);
            restValue += Random.Range(-restValueVariance, restValueVariance);
            nutritionPercentThreshold += Random.Range(-nutritionPercentThresholdVariance, nutritionPercentThresholdVariance);
            hungerDecayRate += Random.Range(-hungerDecayRateVariance, hungerDecayRateVariance);
            sexualUrgeIncrease += Random.Range(-sexualUrgeIncreaseVariance, sexualUrgeIncreaseVariance);
            detectionDistanceBase += Random.Range(-detectionDistanceBaseVariance, detectionDistanceBaseVariance);
            reproductionStaminaPercentCost += Random.Range(-reproductionStaminaPercentCostVariance, reproductionStaminaPercentCostVariance);
            reproductionNutritionPercentCost += Random.Range(-reproductionNutritionPercentCostVariance, reproductionNutritionPercentCostVariance);
            maxAge += Random.Range(-maxAgeVariance, maxAgeVariance);
            ageIncrease += Random.Range(-ageIncreaseVariance, ageIncreaseVariance);
        }
        else if(mutationChance > 0)
        {
            speed += (Random.value > mutationChance)? 0 : Random.Range(-speedVariance, speedVariance);
            maxStamina += (Random.value > mutationChance) ? 0 : Random.Range(-maxStaminaVariance, maxStaminaVariance);
            maxHealth += (Random.value > mutationChance) ? 0 : Random.Range(-maxHealthVariance, maxHealthVariance);
            maxNutrition += (Random.value > mutationChance) ? 0 : Random.Range(-maxNutritionVariance, maxNutritionVariance);
            staminaPercentThreshold += (Random.value > mutationChance) ? 0 : Random.Range(-staminaPercentThresholdVariance, staminaPercentThresholdVariance);
            restValue += (Random.value > mutationChance) ? 0 : Random.Range(-restValueVariance, restValueVariance);
            nutritionPercentThreshold += (Random.value > mutationChance) ? 0 : Random.Range(-nutritionPercentThresholdVariance, nutritionPercentThresholdVariance);
            hungerDecayRate += (Random.value > mutationChance) ? 0 : Random.Range(-hungerDecayRateVariance, hungerDecayRateVariance);
            sexualUrgeIncrease += (Random.value > mutationChance) ? 0 : Random.Range(-sexualUrgeIncreaseVariance, sexualUrgeIncreaseVariance);
            detectionDistanceBase += (Random.value > mutationChance) ? 0 : Random.Range(-detectionDistanceBaseVariance, detectionDistanceBaseVariance);
            reproductionStaminaPercentCost += (Random.value > mutationChance) ? 0 : Random.Range(-reproductionStaminaPercentCostVariance, reproductionStaminaPercentCostVariance);
            reproductionNutritionPercentCost += (Random.value > mutationChance) ? 0 : Random.Range(-reproductionNutritionPercentCostVariance, reproductionNutritionPercentCostVariance);
            maxAge += (Random.value > mutationChance) ? 0 : Random.Range(-maxAgeVariance, maxAgeVariance);
            ageIncrease += (Random.value > mutationChance) ? 0 : Random.Range(-ageIncreaseVariance, ageIncreaseVariance);
        }
        if (clampValues && mutationChance > 0)
        {
            speed = Mathf.Clamp(speed,clampSpeed.x, clampSpeed.y);
            maxStamina = Mathf.Clamp(maxStamina, clampMaxStamina.x, clampMaxStamina.y);
            maxHealth = Mathf.Clamp(maxHealth, clampMaxHealth.x, clampMaxHealth.y);
            maxNutrition = Mathf.Clamp(maxNutrition, clampMaxNutrition.x, clampMaxNutrition.y);
            staminaPercentThreshold = Mathf.Clamp(staminaPercentThreshold, clampStaminaPercentThreshold.x, clampStaminaPercentThreshold.y);
            restValue = Mathf.Clamp(restValue, clampRestValue.x, clampRestValue.y);
            nutritionPercentThreshold = Mathf.Clamp(nutritionPercentThreshold, clampNutritionPercentThreshold.x, clampNutritionPercentThreshold.y);
            hungerDecayRate = Mathf.Clamp(hungerDecayRate, clampHungerDecayRate.x, clampHungerDecayRate.y);
            sexualUrgeIncrease = Mathf.Clamp(sexualUrgeIncrease, clampSexualUrgeIncrease.x, clampSexualUrgeIncrease.y);
            detectionDistanceBase = Mathf.Clamp(detectionDistanceBase, clampDetectionDistanceBase.x, clampDetectionDistanceBase.y);
            reproductionStaminaPercentCost = Mathf.Clamp(reproductionStaminaPercentCost, clampReproductionStaminaPercentCost.x, clampReproductionStaminaPercentCost.y);
            reproductionNutritionPercentCost = Mathf.Clamp(reproductionNutritionPercentCost, clampReproductionNutritionPercentCost.x, clampReproductionNutritionPercentCost.y);
            maxAge = Mathf.Clamp(maxAge, clampMaxAge.x, clampMaxAge.y);
            ageIncrease = Mathf.Clamp(ageIncrease, clampAgeIncrease.x, clampAgeIncrease.y);
        }
    }

    public float GetColorLerp()
    {
        float cocnreteMainConttributio = mainConttribution - 0.5f;
        return Mathf.Lerp(cocnreteMainConttributio, 1, Random.value);
    }
}
