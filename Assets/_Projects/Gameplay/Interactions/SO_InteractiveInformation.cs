using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Interactions
{
    [CreateAssetMenu(menuName = "Asce/Interactions/Interactive Object Information", fileName = "Interactive Object Information")]
    public class SO_InteractiveInformation : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField, TextArea(3, 3)] private string _focusDescription = string.Empty;


        public string Name => _name;
        public string FocusDescription => _focusDescription;
    }
}