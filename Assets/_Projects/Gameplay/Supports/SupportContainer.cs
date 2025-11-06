using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Supports
{
    [System.Serializable]
    public class SupportContainer : IIdentifiable, ISaveable<SupportContainerSaveData>, ILoadable<SupportContainerSaveData>
    {
        public const string PREFIX_ID = "support_container";

        [SerializeField, Readonly] private string _id;
        [SerializeField, Readonly] private string _supportKey;
        [SerializeField, Readonly] private int _level = 0;
        [SerializeField, Readonly] private Cooldown _cooldown = new(10f);

        [Space]
        [SerializeField, Readonly] private Support _supportPrefab;
        [SerializeField, Readonly] private Support _currentSupport;

        public string Id => _id;
        public string SupportKey => _supportKey;

        public int Level => _level;
        public Cooldown Cooldown => _cooldown;
        public Support SupportPrefab => _supportPrefab;
        public SO_SupportInformation Information => IsValid ? _supportPrefab.Information : null;

        public Support CurrentSupport
        {
            get => _currentSupport;
            set => _currentSupport = value;
        }

        public bool IsValid => _supportPrefab != null;
        public bool CurrentSupportIsValid => _currentSupport != null && _currentSupport.gameObject.activeInHierarchy;

        string IIdentifiable.Id
        {
            get => Id;
            set => _id = value;
        }

        public SupportContainer() : this(string.Empty) { }

        public SupportContainer(string id)
        {
            _id = IdGenerator.NewId(PREFIX_ID);
            _supportKey = id;
            this.UpdateSupportReference();
        }

        /// <summary>
        /// Updates support prefab reference based on the current support ID.<br/>
        /// If GameManager or the support is missing, prefab will be set to null.
        /// </summary>
        private void UpdateSupportReference()
        {
            if (!GameManager.HasInstance)
            {
                _supportPrefab = null;
                return;
            }

            Support supportPrefab = GameManager.Instance.AllSupports.Get(_supportKey);
            _supportPrefab = supportPrefab;

            if (_supportPrefab != null && _supportPrefab.Information != null)
            {
                _cooldown.BaseTime = _supportPrefab.Information.Cooldown;
                _cooldown.ToComplete();

                _level = 0;
            }
        }

        SupportContainerSaveData ISaveable<SupportContainerSaveData>.Save()
        {
            return new SupportContainerSaveData()
            {
                id = _id,
                supportKey = _supportKey,
                currentSupportId = _currentSupport != null ? _currentSupport.Id : string.Empty,
                cooldown = _cooldown.CurrentTime,
            };
        }

        void ILoadable<SupportContainerSaveData>.Load(SupportContainerSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            _supportKey = data.supportKey;
            this.UpdateSupportReference();
            _cooldown.CurrentTime = data.cooldown;
            _currentSupport = ComponentUtils.FindComponentById<Support>(data.currentSupportId);
        }
    }
}
