using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(PlayerStats))]
public class Gen_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Label("Testing Tools");
        PlayerStats script = (PlayerStats)target;
        if (GUILayout.Button("Add Oxygen"))
        {
            script.AddOxygen(script.baseOxygenToAdd);
        }
    }
}
