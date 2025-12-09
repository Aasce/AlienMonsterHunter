using Asce.Game;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors.Game
{
    [CustomPropertyDrawer(typeof(CustomValue))]
    public class CustomValueDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Height of one line
            return EditorGUIUtility.singleLineHeight + 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            SerializedProperty nameProp = property.FindPropertyRelative("_name");
            SerializedProperty valueProp = property.FindPropertyRelative("_value");

            float total = position.width;
            float nameWidth = total * 0.7f;
            float valueWidth = total * 0.3f;

            float heigth = position.height - 2;

            Rect nameRect = new(position.x, position.y, nameWidth, heigth);
            Rect valueRect = new(position.x + nameWidth + 2, position.y, valueWidth - 2, heigth);

            EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}