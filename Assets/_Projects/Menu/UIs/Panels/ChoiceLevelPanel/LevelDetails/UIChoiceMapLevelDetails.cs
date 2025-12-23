using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using Asce.Game.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIChoiceMapLevelDetails : UIComponent
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private UILevelBestPlay _bestPlay;
        [SerializeField] private Button _playButton;

        [Space]
        [SerializeField] private Pool<UIMapLevelEnemyInformationItem> _enemyPool = new();
        [SerializeField] private Pool<UIMapLevelConditionInformationItem> _questPool = new();
        [SerializeField] private Pool<UIMapLevelConditionInformationItem> _conditionPool = new();

        [Header("Runtime")]
        [SerializeField, Readonly] private SO_MapLevelInformation _level;

        protected override void RefReset()
        {
            base.RefReset();
        }

        public virtual void Initialize()
        {
            _playButton.onClick.AddListener(PlayButton_OnClick);
        }

        public void Set(SO_MapLevelInformation level)
        {
            this.Unregister();
            _level = level;
            this.Register();
        }

        private void Register()
        {
            if (_level == null) return;

            _levelText.text = $"Level {_level.Level}";
            this.SetLevelEnemies();
            this.SetLevelQuests();
            this.SetLevelConditions();
            this.SetBestPlay();
        }

        private void Unregister()
        {

        }

        private void SetLevelEnemies()
        {
            _enemyPool.Clear(onClear: i => i.Hide());
            foreach (MapLevelEnemy enemy in _level.Enemies)
            {
                UIMapLevelEnemyInformationItem uiEnemy = _enemyPool.Activate();
                if (uiEnemy == null) continue;
                uiEnemy.Set(enemy);

                uiEnemy.RectTransform.SetAsLastSibling();
                uiEnemy.Show();
            }
        }

        private void SetLevelQuests()
        {
            _questPool.Clear(onClear: i => i.Hide());
            foreach (MapLevelGameStateCondition condition in _level.WinConditions)
            {
                UIMapLevelConditionInformationItem uiQuest = _questPool.Activate();
                if (uiQuest == null) continue;
                uiQuest.Set(condition);

                uiQuest.RectTransform.SetAsLastSibling();
                uiQuest.Show();
            }
        }

        private void SetLevelConditions()
        {
            _conditionPool.Clear(onClear: i => i.Hide());
            foreach (MapLevelGameStateCondition condition in _level.LoseConditions)
            {
                UIMapLevelConditionInformationItem uiCondition = _conditionPool.Activate();
                if (uiCondition == null) continue;
                uiCondition.Set(condition);

                uiCondition.RectTransform.SetAsLastSibling();
                uiCondition.Show();
            }
        }

        private void SetBestPlay()
        {
            bool isHasBestPlay = false;
            if (!isHasBestPlay)
            {
                _bestPlay.Hide();
                return;
            }

            _bestPlay.Show();
        }

        private void PlayButton_OnClick()
        {
            MainMenuManager.Instance.PlayNewGame();
        }


    }
}
