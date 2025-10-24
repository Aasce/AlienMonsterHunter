using Asce.Managers;
using UnityEngine;

namespace Asce.SaveLoads
{
    [CreateAssetMenu(menuName = "Asce/SaveLoads/All Save Files", fileName = "All Save Files")]
    public class SO_AllSaveFiles : ScriptableObject
    {
        [SerializeField] private ListObjects<string, SaveFile> _saveFiles = new((saveFile) => 
		{
			if (saveFile == null) return null;
			return saveFile.Name;
		});


        public SaveFile Get(string name) => _saveFiles.Get(name);
		public string GetPath(string name) => this.Get(name)?.Path;
    }
}
