using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public abstract class InteractiveObject : GameComponent, IIdentifiable, ISaveable<InteractiveObjectSaveData>, ILoadable<InteractiveObjectSaveData> 
    {
        public const string PREFIX_ID = "interactive_object";

        [Space]
        [SerializeField, Readonly] private string _id = string.Empty;

        [SerializeField] private SO_InteractiveInformation _information;
        [SerializeField] private bool _isInteractable = true;
        [SerializeField] private float _interactDistance = 1.0f; 

        public string Id => _id;
        public SO_InteractiveInformation Information => _information;
        public bool IsInteractable => _isInteractable;
        public float InteractDistance => _interactDistance;

        string IIdentifiable.Id 
        { 
            get => this.Id; 
            set => _id = value; 
        }

        public virtual void Initialize() 
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
        }
        public virtual void OnLoad() { }
        public virtual void ResetStatus() { }
        public virtual void OnSpawn() { }
        public virtual void OnActive() { }

        public abstract void Interact(GameObject interacter);

        InteractiveObjectSaveData ISaveable<InteractiveObjectSaveData>.Save()
        {
            InteractiveObjectSaveData data = new()
            {
                id = Id,
                name = Information.Name,
                position = transform.position,
                rotation = transform.eulerAngles.z,
            };

            this.OnBeforeSave(data);
            return data;
        }

        void ILoadable<InteractiveObjectSaveData>.Load(InteractiveObjectSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            transform.position = data.position;
            transform.eulerAngles = new Vector3(0f, 0f, data.rotation);
            this.OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(InteractiveObjectSaveData data) { }
        protected virtual void OnAfterLoad(InteractiveObjectSaveData data) { }
    }
}
