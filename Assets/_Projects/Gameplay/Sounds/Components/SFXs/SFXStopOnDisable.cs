using Asce.Core;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SFXStopOnDisable : SFXControlComponent
    {

        private void OnDisable()
        {
            if (ApplicationState.isQuitting) return;
            if (_source == null) return;

            if (SoundManager.Instance != null)
                SoundManager.Instance.StopSFX(_source);

            _source = null;
        }

    }
}
