using Asce.Game.Levelings;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors.Game.Levelings
{
    [CustomEditor(typeof(SO_LevelingInformation), editorForChildClasses: true)]
    public class SO_LevelingInformationEditor : Editor
    {
        private SerializedProperty _maxLevelProp;
        private SerializedProperty _levelingModeProp;
        private SerializedProperty _levelChangesProp;
        private SerializedProperty _uniformGrowthProp;
        private SerializedProperty _specificLevelChangesProp;

        private void OnEnable()
        {
            _maxLevelProp = serializedObject.FindProperty("_maxLevel");
            _levelingModeProp = serializedObject.FindProperty("_levelingMode");
            _levelChangesProp = serializedObject.FindProperty("_levelChanges");
            _uniformGrowthProp = serializedObject.FindProperty("_uniformGrowth");
            _specificLevelChangesProp = serializedObject.FindProperty("_specificLevelChanges");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_maxLevelProp);
            EditorGUILayout.PropertyField(_levelingModeProp);

            EditorGUILayout.Space(5);

            var mode = (LevelingMode)_levelingModeProp.enumValueIndex;

            switch (mode)
            {
                case LevelingMode.PerLevelChanges:
                    DrawPerLevelChanges();
                    break;

                case LevelingMode.UniformGrowth:
                    DrawUniformGrowth();
                    break;

                case LevelingMode.HybridGrowth:
                    DrawHybridGrowth();
                    break;
            }

            DrawPropertiesExcluding(
                serializedObject,
                "_maxLevel",
                "_levelingMode",
                "_levelChanges",
                "_uniformGrowth",
                "_specificLevelChanges",
                "m_Script"
            );

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPerLevelChanges()
        {
            EditorGUILayout.LabelField("Per-Level Modifications", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_levelChangesProp, includeChildren: true);
        }

        private void DrawUniformGrowth()
        {
            EditorGUILayout.LabelField("Uniform Growth", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_uniformGrowthProp, includeChildren: true);
        }

        private void DrawHybridGrowth()
        {
            EditorGUILayout.LabelField("Hybrid Growth Settings", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Uniform Growth (Fallback)", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_uniformGrowthProp, includeChildren: true);

            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("Specific Level Modifications", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_specificLevelChangesProp, includeChildren: true);
        }
    }
}
