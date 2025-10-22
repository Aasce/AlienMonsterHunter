using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class Support : GameComponent, IIdentifiable, ISaveable<SupportSaveData>, ILoadable<SupportSaveData>
    {
        public const string PREFIX_ID = "support";
        [SerializeField, Readonly] protected string _id;

        [Space]
        [SerializeField] protected SO_SupportInformation _information;
        [SerializeField] protected Vector2 _callPosition;


        public string Id => _id;
        public SO_SupportInformation Information => _information;
        public virtual Vector2 CallPosition
        {
            get => _callPosition;
            set => _callPosition = value;
        }

        string IIdentifiable.Id
        {
            get => _id;
            set => _id = value;
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Initialize() 
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
            
        }

        public virtual void ResetStatus() { }

        public virtual void OnSpawn() { }

        public virtual void OnActive() { }
        public virtual void Recall() { }
        public virtual void OnLoad() { }


        SupportSaveData ISaveable<SupportSaveData>.Save()
        {
            SupportSaveData data = new()
            {
                id = this.Id,
                nameId = this.Information.Id,
                position = transform.position,
                rotation = transform.eulerAngles.z,
                callPosition = this.CallPosition
            };
            this.OnBeforeSave(data);
            return data;
        }

        void ILoadable<SupportSaveData>.Load(SupportSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            transform.position = data.position;
            transform.eulerAngles = new Vector3(0f, 0f, data.rotation);
            _callPosition = data.callPosition;
            this.OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(SupportSaveData data) { }
        protected virtual void OnAfterLoad(SupportSaveData data) { }
    }
}
