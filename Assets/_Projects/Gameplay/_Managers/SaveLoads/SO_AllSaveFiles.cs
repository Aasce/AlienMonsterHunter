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

        [SerializeField]
        private ListObjects<string, SaveFile> _saveFolders = new((saveFolder) =>
        {
            if (saveFolder == null) return null;
            return saveFolder.Name;
        });


        public SaveFile Get(string name) => _saveFiles.Get(name);
		public string GetPath(string name) => this.Get(name)?.Path;


        public SaveFile GetFolder(string name) => _saveFolders.Get(name);
		public string GetFolderPath(string name) => this.GetFolder(name)?.Path;
    }
}
