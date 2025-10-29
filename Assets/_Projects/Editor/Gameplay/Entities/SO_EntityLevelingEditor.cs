using Asce.Game.Levelings;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors.Game.Levelings
{
    [CustomEditor(typeof(SO_EntityLeveling), editorForChildClasses: true)]
    public class SO_EntityLevelingEditor : Editor
    {
        private SerializedProperty _maxLevelProp;
        private SerializedProperty _levelingModeProp;
        private SerializedProperty _levelChangesProp;
        private SerializedProperty _uniformGrowthProp;

        private void OnEnable()
        {
            _maxLevelProp = serializedObject.FindProperty("_maxLevel");
            _levelingModeProp = serializedObject.FindProperty("_levelingMode");
            _levelChangesProp = serializedObject.FindProperty("_levelChanges");
            _uniformGrowthProp = serializedObject.FindProperty("_uniformGrowth");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Level Settings", EditorStyles.boldLabel);
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
            }

            DrawPropertiesExcluding(
                serializedObject,
                "_maxLevel",
                "_levelingMode",
                "_levelChanges",
                "_uniformGrowth",
                "m_Script" // always exclude script reference
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
    }
}
