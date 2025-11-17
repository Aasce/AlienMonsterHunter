using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    [RequireComponent(typeof(EntityView))]
    public class FirebaitBomb_MachineView : GameComponent
    {
        private static readonly int _scrollSpeedID = Shader.PropertyToID("_Scroll_Speed");

        [SerializeField, Readonly]
        private FirebaitBomb_Machine _machine;

        [SerializeField, Readonly]
        private EntityView _view;

        [Space]
        [SerializeField]
        private List<SpriteRenderer> _scrollingRenderers = new();

        [Header("Scroll Settings")]
        [Tooltip("Multiplier to convert machine movement speed into scroll speed.")]
        [SerializeField]
        private float _scrollSpeedMultiplier = 1f;

        private MaterialPropertyBlock _block;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _machine);
            this.LoadComponent(out _view);
        }

        private void Awake()
        {
            _block = new MaterialPropertyBlock();
        }

        private void LateUpdate()
        {
            float scrollSpeed = CalculateScrollSpeed();
            ApplyScrollSpeed(scrollSpeed);
        }

        /// <summary>
        /// Convert machine movement speed to shader scroll speed. 
        /// If the machine is stopped, returns 0.
        /// </summary>
        private float CalculateScrollSpeed()
        {
            if (_machine.IsStopMoving)
                return 0f;

            float machineSpeed = _machine.Stats.Speed.FinalValue;
            return -machineSpeed * _scrollSpeedMultiplier;
        }

        /// <summary>
        /// Apply scroll speed to all SpriteRenderers using MaterialPropertyBlock.
        /// </summary>
        private void ApplyScrollSpeed(float speed)
        {
            foreach (var renderer in _scrollingRenderers)
            {
                if (renderer == null)
                    continue;

                renderer.GetPropertyBlock(_block);
                _block.SetFloat(_scrollSpeedID, speed);
                renderer.SetPropertyBlock(_block);
            }
        }
    }
}
