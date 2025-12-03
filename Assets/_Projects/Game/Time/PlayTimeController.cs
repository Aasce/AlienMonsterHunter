using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.MainGame
{
    public class PlayTimeController : GameComponent
    {
		[SerializeField] private float _elapsedTime = 0f;
		[SerializeField] private bool _isCounting = true;

		[Space]
		[SerializeField, Readonly] private Cooldown _callEventCooldown = new(0.1f);

		public event Action OnPaused;
		public event Action OnResumed;
		public event Action<float> OnElapsedTimeChanged;

		/// <summary> Total elapsed play time in seconds. </summary>
		public float ElapsedTime 
		{ 
			get => _elapsedTime; 
			private set => _elapsedTime = value; 
		}

		public void Initialize() 
		{
			
		}

		private void Update()
		{
			if (!_isCounting) return;

			ElapsedTime += Time.deltaTime;
			_callEventCooldown.Update(Time.deltaTime);
			if (_callEventCooldown.IsComplete)
			{
				_callEventCooldown.Reset();
                OnElapsedTimeChanged?.Invoke(ElapsedTime);
            }
        }

		/// <summary> Set the starting time. </summary>
		public void SetElapsedTime(float seconds)
		{
			ElapsedTime = seconds;
		}

		/// <summary> Pause the time counter. </summary>
		public void Pause()
		{
			_isCounting = false;
			OnPaused?.Invoke();
		}

		/// <summary> Resume the time counter. </summary>
		public void Resume()
		{
			_isCounting = true;
			OnResumed?.Invoke();
		}

        /// <summary> Returns formatted time (MM:SS.ff). </summary>
        public string GetFormattedTime()
        {
            float time = ElapsedTime;

            int totalSeconds = Mathf.FloorToInt(time);
            int minutes = totalSeconds / 60;

            float secondsWithFraction = time % 60f; // Seconds with centiseconds (2 digits)

            return $"{minutes:00}:{secondsWithFraction:00.0}";
        }

    }
}
