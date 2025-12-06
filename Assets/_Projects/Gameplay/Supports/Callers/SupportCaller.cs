using Asce.Core;
using Asce.Core.Attributes;
using Asce.Game.Enviroments;
using Asce.Game.Players;
using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UIElements;

namespace Asce.Game.Supports
{
    public class SupportCaller : GameComponent, IIdentifiable, ISaveable<SupportCallerSaveData>, ILoadable<SupportCallerSaveData>
    {
        public const string PREFIX_ID = "support_caller";
        [SerializeField, Readonly] private string _id = string.Empty;

        [Space]
        [SerializeField, Readonly] private List<SupportContainer> _supports = new();
        private ReadOnlyCollection<SupportContainer> _supportsReadonly;

        public event Action OnSupportChanged;

        public string Id => _id;
        public ReadOnlyCollection<SupportContainer> Supports => _supportsReadonly ??= _supports.AsReadOnly();

        string IIdentifiable.Id 
        {
            get => this.Id; 
            set => _id = value; 
        }

        public void Initialize(List<string> _supportKeys)
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
            if (_supportKeys == null) return;
            _supports.Clear();
            foreach (string id in _supportKeys)
            {
                if (string.IsNullOrEmpty(id)) continue;
                SupportContainer container = new(id);
                if (!container.IsValid)
                {
                    Debug.LogError($"Support with id {id} not exist.", this);
                    continue;
                }

                PlayerManager.Instance.Progress.SupportsProgress.ApplyTo(container);
                _supports.Add(container);
            }

            this.OnSupportChanged?.Invoke();
            StartCoroutine(WaitSpawnSupportOnStart());
        }

        private IEnumerator WaitSpawnSupportOnStart()
        {
            yield return null;
            this.SpawnSupportOnStart();
        }
        private void SpawnSupportOnStart()
        {
            foreach (SupportContainer container in _supports)
            {
                if (!container.IsValid) continue;
                if (!container.SupportPrefab.Information.SpawnOnStart) continue;
                this.NewCall(container, Vector2.zero);
                container.Cooldown.ToComplete();
            }
        }

        public void OnLoad()
        {

        }

        private void Update()
        {
            foreach (SupportContainer container in _supports)
            {
                if (container == null) continue;
                if (!container.IsValid) continue;
                container.Cooldown.Update(Time.deltaTime);
                if (container.CurrentSupport != null)
                {
                    if (!container.CurrentSupport.gameObject.activeInHierarchy)
                    {
                        container.CurrentSupport = null;
                        container.Cooldown.SetBaseTime(container.Information.Cooldown);
                    }
                }
            }
        }

        public void Call(int index, Vector2 position)
        {
            if (index < 0 || index >= _supports.Count) return;
            SupportContainer container = _supports[index];
            if (container == null) return;
            if (!container.IsValid) return;
            if (!container.Cooldown.IsComplete) return;

            if (container.CurrentSupportIsValid) this.Recall(container, position);
            else this.NewCall(container, position);
        }

        private void Recall(SupportContainer container, Vector2 position)
        {
            Support currentSupport = container.CurrentSupport;
            currentSupport.CallPosition = position;
            currentSupport.Recall();

            container.Cooldown.SetBaseTime(container.Information.CooldownOnRecall);
        }

        private void NewCall(SupportContainer container, Vector2 position)
        {
            Support spawnSupport = SupportController.Instance.Spawn(container.SupportKey);
            if (spawnSupport == null) return;

            this.SetSupportPosition(spawnSupport, spawnSupport.Information.SpawnPointType);
            spawnSupport.CallPosition = position;
            spawnSupport.Leveling.SetLevel(container.Level);
            spawnSupport.gameObject.SetActive(true);
            spawnSupport.OnActive();
            container.CurrentSupport = spawnSupport;

            container.Cooldown.SetBaseTime(container.Information.CooldownOnRecall);
        }

        private void SetSupportPosition(Support support, SpawnPointType type)
        {
            switch (type)
            {
                case SpawnPointType.SpawnPoint:
                    support.transform.position = EnviromentController.Instance.SpawnPoints.SupportSpawnPoint;
                    return;
                case SpawnPointType.SpawnHolder:
                    support.transform.position = EnviromentController.Instance.SpawnPoints.GetEmptyHolder();
                    return;
            }
        }

        SupportCallerSaveData ISaveable<SupportCallerSaveData>.Save()
        {
            SupportCallerSaveData supportCallerData = new()
            {
                id = this._id
            };
            foreach (SupportContainer container in _supports)
            {
                SupportContainerSaveData containerData = (container as ISaveable<SupportContainerSaveData>).Save();
                supportCallerData.supports.Add(containerData);
            }
            return supportCallerData;
        }

        void ILoadable<SupportCallerSaveData>.Load(SupportCallerSaveData data)
        {
            if (data == null) return;
            _id = data.id;

            _supports.Clear();
            foreach (SupportContainerSaveData containerData in data.supports)
            {
                SupportContainer container = new();
                (container as ILoadable<SupportContainerSaveData>).Load(containerData);
                _supports.Add(container);
            }

            this.OnSupportChanged?.Invoke();
        }
    }
}