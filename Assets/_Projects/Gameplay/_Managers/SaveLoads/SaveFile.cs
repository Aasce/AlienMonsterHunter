using UnityEngine;

namespace Asce.SaveLoads
{
    [System.Serializable]
    public class SaveFile
    {
        [SerializeField] private string _name;
        [SerializeField] private string _path;

		public string Name => _name;
		public string Path => _path;
		
		public SaveFile(string name, string path)
        {
            _name = name;
            _path = path;
        }
    }
}
