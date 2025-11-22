using Asce.Game.UIs;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors.Game.UIs
{
    [CustomEditor(typeof(UICurrencies))]
    public class UICurrenciesEditor : Editor
    {
        private SerializedProperty _allCurrencies;
        private SerializedProperty _includeCurrencies;
        private SerializedProperty _excludeCurrencies;

        private void OnEnable()
        {
            _allCurrencies = serializedObject.FindProperty("_allCurrencies");
            _includeCurrencies = serializedObject.FindProperty("_includeCurrencies");
            _excludeCurrencies = serializedObject.FindProperty("_excludeCurrencies");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw default pool field
            DrawPropertiesExcluding(serializedObject, "_includeCurrencies", "_excludeCurrencies", "_allCurrencies");

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_allCurrencies);

            if (_allCurrencies.boolValue)
            {
                // Show only exclude list
                EditorGUILayout.PropertyField(_excludeCurrencies, true);
            }
            else
            {
                // Show only include list
                EditorGUILayout.PropertyField(_includeCurrencies, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
