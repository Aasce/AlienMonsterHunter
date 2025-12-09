using Asce.Core;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors
{
    [CustomPropertyDrawer(typeof(ListObjects<,>), true)]
    public class ListObjectsDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty listProp = property.FindPropertyRelative("_list");

            // Height of list including children
            return EditorGUI.GetPropertyHeight(listProp, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty listProp = property.FindPropertyRelative("_list");

            // Draw the list directly, no foldout for the wrapper class
            EditorGUI.PropertyField(position, listProp, label, true);

            EditorGUI.EndProperty();
        }
    }
}
