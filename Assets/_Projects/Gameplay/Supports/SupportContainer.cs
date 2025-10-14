using Asce.Game.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Supports
{
    [System.Serializable]
    public class SupportContainer
    {
        [SerializeField] private string _supportId;
        [SerializeField] private Support _supportPrefab;
        [SerializeField] private Cooldown _cooldown = new(10f);

        [Space]
        [SerializeField] private Support _currentSupport = null;

        public string SupportId
        {
            get => _supportId;
            set
            {
                if (_supportId == value) return;
                if (GameManager.Instance == null)
                {
                    _supportId = value;
                    _supportPrefab = null;
                    return;
                }

                Support supportPrefab = GameManager.Instance.AllSupports.Get(value);
                if (supportPrefab == null || supportPrefab.Information == null)
                {
                    _supportId = value;
                    _supportPrefab = null;
                    return;
                }

                _supportId = value;
                _supportPrefab = supportPrefab;
            }
        }

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

        public SupportContainer() { }
        public SupportContainer(string name)
        {
            SupportId = name;
            if (_supportPrefab != null)
            {
                _cooldown.BaseTime = _supportPrefab.Information.Cooldown;
                _cooldown.ToComplete();
            }
        }
    }
}