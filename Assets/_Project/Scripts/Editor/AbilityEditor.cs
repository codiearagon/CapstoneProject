using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ability))]
public class AbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Ability ability = target as Ability;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Details", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AbilityName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Icon"));

        EditorGUILayout.LabelField("Description");
        ability.Description = EditorGUILayout.TextArea(ability.Description, GUILayout.Height(100f));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Affinity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AttackMultiplier"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ManaCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CooldownTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AlwaysActive"));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Ability Type", EditorStyles.boldLabel);
        ability.Type =  (AbilityType) EditorGUILayout.EnumPopup("Type", ability.Type);

        switch (ability.Type)
        {
            case AbilityType.Projectile:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ProjectileProperties"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
