using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Unit))]
public class UnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Unit unit = (Unit)target;
        if (GUILayout.Button("Set Hungry"))
        {
            unit.Nutrition = unit.NutritionPercentThreshold * unit.MaxNutrition;
        }
        if (GUILayout.Button("Set Tired"))
        {
            unit.Stamina = unit.StaminaPercentThreshold * unit.MaxStamina;
        }
        if (GUILayout.Button("Set Horny"))
        {
            unit.SexualUrge = 500;
        }

        DrawDefaultInspector();
    }
}
