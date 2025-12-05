using Asce.Core;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.VFXs
{
    /// <summary>
    ///     Controls spawning popup texts (damage, heal, etc.) on objects with rate limits.
    /// </summary>
    public class PopupTextController : MonoBehaviourSingleton<PopupTextController>
    {
        [Header("Configs")]
        [SerializeField] private SO_PopupTextColor _popupTextColor;

        [Header("VFX Settings")]
        [SerializeField] private string _popupTextVFXName = string.Empty;
        [SerializeField] private Vector2 _offset = Vector2.up * 1.0f;

        [Header("Timing Settings")]
        [SerializeField] private Cooldown _updateCooldown = new(0.05f);
        [SerializeField] private float _delay = 0.15f;

        private readonly Dictionary<Transform, Queue<PopupTextData>> _popupQueues = new();
        private readonly Dictionary<Transform, float> _nextPopupTime = new();
        private readonly List<Transform> _toRemove = new();

        public SO_PopupTextColor PopupTextColor => _popupTextColor;

        private void Update()
        {
            _updateCooldown.Update(Time.deltaTime);
            if (!_updateCooldown.IsComplete)
                return;

            ProcessPopupQueues();
            _updateCooldown.Reset();
        }

        /// <summary>
        ///     Adds a text to the queue for a specific object.
        /// </summary>
        public void EnqueuePopupText(Transform target, string text)
        {
            this.EnqueuePopupText(target, new PopupTextData(text, Color.white));
        }

        /// <summary>
        ///     Adds a popup text data to the queue for a specific object.
        /// </summary>
        public void EnqueuePopupText(Transform target, PopupTextData data)
        {
            if (target == null || string.IsNullOrWhiteSpace(data.Text))
                return;

            if (!_popupQueues.TryGetValue(target, out Queue<PopupTextData> queue) || queue == null)
            {
                queue = new Queue<PopupTextData>();
                _popupQueues[target] = queue;
            }

            queue.Enqueue(data);
        }

        /// <summary>
        ///     Processes all popup queues and spawns popup texts when ready.
        /// </summary>
        private void ProcessPopupQueues()
        {
            float now = Time.time;

            foreach (var kvp in _popupQueues)
            {
                Transform target = kvp.Key;

                // Skip and mark for removal if destroyed
                if (target == null)
                {
                    _toRemove.Add(kvp.Key);
                    continue;
                }

                Queue<PopupTextData> queue = kvp.Value;
                if (queue == null || queue.Count == 0)
                    continue;

                if (!_nextPopupTime.TryGetValue(target, out float nextTime))
                    _nextPopupTime[target] = 0f;

                if (now < nextTime)
                    continue;

                if (!queue.TryDequeue(out PopupTextData data))
                    continue;

                SpawnPopupText(target.position + (Vector3)_offset, data);

                _nextPopupTime[target] = now + _delay;
            }

            // Clean up destroyed targets
            foreach (Transform dead in _toRemove)
            {
                _popupQueues.Remove(dead);
                _nextPopupTime.Remove(dead);
            }
            _toRemove.Clear();
        }

        /// <summary>
        ///     Spawns a popup text VFX at a given position.
        /// </summary>
        private void SpawnPopupText(Vector2 position, PopupTextData data)
        {
            if (string.IsNullOrWhiteSpace(_popupTextVFXName))
                return;

            PopupTextVFXObject popup = VFXController.Instance.Spawn(_popupTextVFXName, position) as PopupTextVFXObject;
            if (popup == null)
                return;

            popup.SetText(data.Text, data.Color, data.Size, data.Style);
        }
    }
}
