using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public abstract class Ability : GameComponent, IIdentifiable, ISaveable<AbilitySaveData>, ILoadable<AbilitySaveData>
    {
        public const string PREFIX_ID = "Ability";

        [SerializeField, Readonly] protected string _id = string.Empty;
        [SerializeField] protected SO_AbilityInformation _information;
        [SerializeField] protected GameObject _owner;
        [SerializeField] protected Cooldown _despawnTime = new (10f);

        public string Id => _id;
        public SO_AbilityInformation Information => _information;
        public GameObject Owner
        {
            get => _owner;
            set => _owner = value;
        }
        public Cooldown DespawnTime => _despawnTime;

        string IIdentifiable.Id 
        {
            get => Id;
            set => _id = value;
        }


        protected virtual void Start()
        {
        }

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(this._id)) this._id = IdGenerator.NewId(PREFIX_ID);
            DespawnTime.SetBaseTime(Information.DaspawnTime);

        }

        public virtual void ResetStatus()
        {
            this.DespawnTime.Reset();
        }

        public virtual void OnSpawn()
        {

        }

        public virtual void OnActive() { }

        public virtual void OnDespawn()
        {

        }

        AbilitySaveData ISaveable<AbilitySaveData>.Save()
        {
            AbilitySaveData abilityData = new ()
            {
                id = this.Id,
                name = Information.Name,
                ownerId = this.Owner.GetId(),
                despawnTime = this.DespawnTime.CurrentTime,
                position = this.transform.position,
                rotation = this.transform.eulerAngles.z,
            };
            this.OnBeforeSave(abilityData);
            return abilityData;
        }

        void ILoadable<AbilitySaveData>.Load(AbilitySaveData data)
        {
            if (data == null) return;
            this._id = data.id;
            this._despawnTime.CurrentTime = data.despawnTime;
            this.transform.position = data.position;
            this.transform.eulerAngles = new Vector3(0f, 0f, data.rotation);
            this._owner = ComponentUtils.FindGameObjectById(data.ownerId);

            this.OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(AbilitySaveData data) { }
        protected virtual void OnAfterLoad(AbilitySaveData data) { }
    }
}