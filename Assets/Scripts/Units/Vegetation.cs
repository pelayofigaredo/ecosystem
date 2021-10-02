using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Food))]
[RequireComponent(typeof(Collider))]

public class Vegetation : MonoBehaviour
{
    [SerializeField]
    new Collider collider;
    [SerializeField]
    new MeshRenderer renderer;
    [SerializeField]
    ScaleModifier scaleModifier;
    [SerializeField]
    float regenerationTime = 20;
    [SerializeField]
    bool onlyOnce = false;

    Food food;
    float nutrition;

    private void Awake()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider>();
        }
        if(renderer == null)
        {
            renderer = GetComponentInChildren<MeshRenderer>();
        }
        if(scaleModifier == null)
        {
            scaleModifier = GetComponentInChildren<ScaleModifier>();
        }
       food =  GetComponent<Food>();
        food.OnBeenEaten += Consume;
        nutrition = food.NutritionalValue;
    }

    void Consume()
    {
        collider.enabled = false;
        renderer.enabled = false;
        food.NutritionalValue = 0;

        if (!onlyOnce)
        {
            scaleModifier.transform.localScale = Vector3.zero;
            StartCoroutine(Regenerate());
        }
        else
        {
            Destroy(gameObject, 1);
        }

    }

    IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(regenerationTime);
        collider.enabled = true;
        renderer.enabled = true;
        food.NutritionalValue = nutrition;
        scaleModifier.ModifyToOriginal();
    }
}
