using Asce.MainGame.Managers;
using Asce.Managers.UIs;
using TMPro;
using UnityEngine;

namespace Asce.MainGame.UIs.HUDs
{
    public class UIPlaytime : UIObject
    {
        [SerializeField] private TextMeshProUGUI _playtimeText;


        public void Initialize()
        {
            MainGameManager.Instance.PlayTimeController.OnElapsedTimeChanged += PlayTimeController_OnElapsedTimeChanged;
        }

        private void PlayTimeController_OnElapsedTimeChanged(float elapseTime)
        {
            int totalSeconds = Mathf.FloorToInt(elapseTime);
            int minutes = totalSeconds / 60;

            float secondsWithFraction = elapseTime % 60f;

            int seconds = Mathf.FloorToInt(secondsWithFraction);
            int milisecond = Mathf.FloorToInt((secondsWithFraction - seconds) * 10f);

            float smallSize = _playtimeText.fontSize * 0.8f;
            _playtimeText.text = $"{minutes:00}:{seconds:00}.<size={smallSize}>{milisecond:0}</size>";
        }

    }
}
