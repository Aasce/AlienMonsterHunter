using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class InteractiveObjectSaveData : SaveDataWithCustoms
    {
        public string id;
        public string name;
        public Vector2 position;
        public float rotation;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is InteractiveObjectSaveData interactiveObjectData)
            {
                id = interactiveObjectData.id;
                name = interactiveObjectData.name;
                position = interactiveObjectData.position;
                rotation = interactiveObjectData.rotation;

            }
        }
    }

}