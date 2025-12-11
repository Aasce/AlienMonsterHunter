using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Sounds
{
    [CreateAssetMenu(menuName = "Asce/Sounds/All Sounds", fileName = "All Sounds")]
    public class SO_AllSounds : ScriptableObject
    {
        [SerializeField] protected ListObjects<string, SO_SoundParameters> _audio = new(audio =>
        {
            if (audio == null) return null;
            return audio.Name;
        });

        public ReadOnlyCollection<SO_SoundParameters> Audio => _audio.List;
        public SO_SoundParameters Get(string name) => _audio.Get(name);

    }
}