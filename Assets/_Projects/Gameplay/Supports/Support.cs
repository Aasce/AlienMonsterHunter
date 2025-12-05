using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class Support : GameComponent, IIdentifiable, ISaveable<SupportSaveData>, ILoadable<SupportSaveData>
    {
        public const string PREFIX_ID = "support";
        [SerializeField, Readonly] protected string _id;

        [Space]
        [SerializeField] protected SO_SupportInformation _information;
        [SerializeField, Readonly] protected Leveling _leveling;
        [SerializeField, Readonly] protected Vector2 _callPosition;


        public string Id => _id;
        public SO_SupportInformation Information => _information;
        public Leveling Leveling => _leveling;
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

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _leveling);
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Initialize() 
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
            Leveling.Initialize(Information.Leveling);

            Leveling.OnLevelSetted += Leveling_OnLevelSetted;
            Leveling.OnLevelUp += Leveling_OnLevelUp;
        }

        public virtual void ResetStatus() { }

        public virtual void OnSpawn() { }

        public virtual void OnActive() { }
        public virtual void Recall() { }
        public virtual void OnLoad() { }

        protected virtual void LevelTo(int newLevel)
        {

        }

        protected virtual void Leveling_OnLevelSetted(int newLevel)
        {
            for (int i = 1; i <= newLevel; i++)
            {
                this.LevelTo(i);
            }
        }

        protected virtual void Leveling_OnLevelUp(int newLevel) => LevelTo(newLevel);


        SupportSaveData ISaveable<SupportSaveData>.Save()
        {
            SupportSaveData data = new()
            {
                id = this.Id,
                nameId = this.Information.Key,
                level = (Leveling as ISaveable<LevelingSaveData>).Save(),
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

            (Leveling as ILoadable<LevelingSaveData>).Load(data.level);

            transform.position = data.position;
            transform.eulerAngles = new Vector3(0f, 0f, data.rotation);
            _callPosition = data.callPosition;
            this.OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(SupportSaveData data) { }
        protected virtual void OnAfterLoad(SupportSaveData data) { }
    }
}
