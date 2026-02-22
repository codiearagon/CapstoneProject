using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ability))]
public class AbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Ability ability = target as Ability;

        //EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Details", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.AbilityName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.Icon"));

        EditorGUILayout.LabelField("Description");
        ability.Properties.Description = EditorGUILayout.TextArea(ability.Properties.Description, GUILayout.Height(100f));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.Affinity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.AttackMultiplier"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.ManaCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.CooldownTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.AlwaysActive"));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Ability Type", EditorStyles.boldLabel);
        ability.Properties.Type =  (AbilityType) EditorGUILayout.EnumPopup("Type", ability.Properties.Type);

        switch (ability.Properties.Type)
        {
            case AbilityType.Projectile:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Properties.ProjectileProperties"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
