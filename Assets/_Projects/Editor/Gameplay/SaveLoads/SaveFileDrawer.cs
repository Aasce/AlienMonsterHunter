using Asce.SaveLoads;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors.SaveLoads
{
    [CustomPropertyDrawer(typeof(SaveFile))]
    public class SaveFileDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 2 lines (Name + Path) + small spacing
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Disable foldout by ignoring label and drawing fields directly
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty nameProp = property.FindPropertyRelative("_name");
            SerializedProperty pathProp = property.FindPropertyRelative("_path");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect nameRect = new Rect(position.x, position.y, position.width, lineHeight);
            Rect pathRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);

            EditorGUI.PropertyField(nameRect, nameProp, new GUIContent("Name"));
            EditorGUI.PropertyField(pathRect, pathProp, new GUIContent("Path"));

            EditorGUI.EndProperty();
        }
    }
}
