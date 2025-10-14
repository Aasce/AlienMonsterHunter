using Asce.Managers;
using Asce.Managers.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class SupportCaller : GameComponent
    {
        [SerializeField, Readonly] private List<SupportContainer> _supports = new();
        [SerializeField] private Vector2 _spawnPoint = Vector2.zero;

        public event Action OnInitializeCompleted;


        public Vector2 SpawnPoint
        {
            get => _spawnPoint;
            set => _spawnPoint = value;
        }

        private void Update()
        {
            foreach (SupportContainer container in _supports)
            {
                if (container == null) continue;
                if (!container.IsValid) continue;
                container.Cooldown.Update(Time.deltaTime);
            }
        }

        public void Initialize(List<string> _supportIds)
        {
            if (_supportIds == null) return;
            _supports.Clear();
            foreach (string id in _supportIds)
            {
                if (string.IsNullOrEmpty(id)) continue;
                SupportContainer container = new(id);
                if (!container.IsValid)
                {
                    Debug.LogError($"Support with id {id} not exist.", this);
                    continue;
                }

                _supports.Add(container);
            }

            this.OnInitializeCompleted?.Invoke();
        }

        public void Call(int index, Vector2 position)
        {
            if (index < 0 || index >= _supports.Count) return;
            SupportContainer container = _supports[index];
            if (container == null) return;
            if (!container.IsValid) return;
            if (!container.Cooldown.IsComplete) return;

            if (container.CurrentSupportIsValid) 
            {
                Support currentSupport = container.CurrentSupport;
                currentSupport.CallPosition = position;
                currentSupport.Recall();

                container.Cooldown.SetBaseTime(container.Information.CooldownOnRecall);
            }
            else
            {
                Support spawnSupport = SupportController.Instance.Spawn(container.SupportId);
                if (spawnSupport == null) return;

                spawnSupport.transform.position = SpawnPoint;
                spawnSupport.CallPosition = position;
                spawnSupport.gameObject.SetActive(true);
                spawnSupport.OnActive();
                container.CurrentSupport = spawnSupport;

                container.Cooldown.SetBaseTime(container.Information.Cooldown);
            }
        }
    }
}