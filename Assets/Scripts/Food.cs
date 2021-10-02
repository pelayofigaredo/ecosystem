using UnityEngine;

public class Food : MonoBehaviour
{
    public System.Action OnBeenEaten;
    [SerializeField]
    private float nutritionalValue = 100;
    public float NutritionalValue { get => nutritionalValue; set => nutritionalValue = value; }

    public void Eated()
    {
        OnBeenEaten?.Invoke();
    }
}
