using Asce.Game.Levelings;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors.Game.Levelings
{
    [CustomPropertyDrawer(typeof(LevelModification))]
    public class LevelModificationDrawer : PropertyDrawer
    {
        private const float Spacing = 2f;

        // Cached relative property names
        private static readonly string TargetKeyPropName = "_targetKey";
        private static readonly string DeltaValuePropName = "_deltaValue";
        private static readonly string TypePropName = "_type";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + Spacing * 3;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get sub-properties once (constant lookup)
            SerializedProperty targetKeyProp = property.FindPropertyRelative(TargetKeyPropName);
            SerializedProperty deltaValueProp = property.FindPropertyRelative(DeltaValuePropName);
            SerializedProperty typeProp = property.FindPropertyRelative(TypePropName);

            int prevIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // --- Line 1: Key ---
            Rect keyRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(keyRect, targetKeyProp, new GUIContent("Key"));

            // --- Line 2: Value + Type ---
            float lineY = keyRect.yMax + Spacing;
            float valueWidth = (position.width - Spacing) * 0.7f;

            Rect valueRect = new(position.x, lineY, valueWidth, EditorGUIUtility.singleLineHeight);
            Rect typeRect = new(valueRect.xMax + Spacing, lineY, position.width - valueRect.width - Spacing, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(valueRect, deltaValueProp, new GUIContent("Value"));
            EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);

            EditorGUI.indentLevel = prevIndent;
            EditorGUI.EndProperty();
        }
    }
}
