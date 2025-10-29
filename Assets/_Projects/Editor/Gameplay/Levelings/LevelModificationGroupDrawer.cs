using Asce.Game.Levelings;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors.Game.Levelings
{
    [CustomPropertyDrawer(typeof(LevelModificationGroup))]
    public class LevelModificationGroupDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if this property is an element inside a list/array
            bool isInList = property.propertyPath.Contains("[") && property.propertyPath.Contains("]");

            if (isInList)
            {
                int index = GetElementIndex(property);
                if (index >= 0)
                {
                    label = new GUIContent($"Level {index + 1}");
                }
            }

            EditorGUI.PropertyField(position, property, label, includeChildren: true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        private int GetElementIndex(SerializedProperty property)
        {
            string path = property.propertyPath;
            int start = path.IndexOf("[") + 1;
            int end = path.IndexOf("]", start);

            if (start > 0 && end > start && int.TryParse(path[start..end], out int index))
                return index;

            return -1;
        }
    }
}
