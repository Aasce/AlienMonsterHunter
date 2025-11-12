using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.Entities
{
    public class EntityUI : GameComponent
    {
        [SerializeField, Readonly] protected Canvas _canvas;
        [SerializeField, Readonly] protected Entity _owner;

        [Space]
        [SerializeField] protected Slider _healthBar;
        [SerializeField] protected TextMeshProUGUI _levelText;

        [Header("Config")]
        [SerializeField] protected Vector2 _offset = Vector2.up;

        public Canvas Canvas => _canvas;
        public Entity Owner
        {
            get => _owner;
            set => _owner = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
        }

        public virtual void Initialize()
        {
            _healthBar.maxValue = Owner.Stats.Health.FinalValue;
            _healthBar.value = Owner.Stats.Health.CurrentValue;
            _levelText.text = Owner.Leveling.CurrentLevel.ToString();

            Owner.Stats.Health.OnFinalValueChanged += Health_OnFinalValueChanged;
            Owner.Stats.Health.OnCurrentValueChanged += Health_OnCurrentValueChanged;
            Owner.Leveling.OnLevelChanged += Leveling_OnLevelChanged;
        }

        public virtual void ResetStatus()
        {

        }

        public virtual void OnLoad()
        {

        }

        protected virtual void LateUpdate()
        {
            Vector2 position = (Vector2)Owner.transform.position + _offset;
            _canvas.transform.SetPositionAndRotation(position, Quaternion.identity);
        }

        protected virtual void Health_OnFinalValueChanged(float oldValue, float newValue)
        {
            _healthBar.maxValue = newValue;
        }

        protected virtual void Health_OnCurrentValueChanged(float oldValue, float newValue)
        {
            _healthBar.value = newValue;
        }

        protected virtual void Leveling_OnLevelChanged(int newLevel)
        {
            _levelText.text = newLevel.ToString();
        }

    }
}
