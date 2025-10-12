using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Asce.Managers.UIs
{
    public class UIManager : DontDestroyOnLoadSingleton<UIManager>
    {
        private readonly List<RaycastResult> _results = new();
        private PointerEventData _eventData;

        /// <summary>
        ///     Returns true only if pointer is over a UI element in Screen Space canvas.
        /// </summary>
        public bool IsPointerOverScreenUI()
        {
            if (EventSystem.current == null) return false;
            if (!EventSystem.current.IsPointerOverGameObject()) return false;

            // Create once, reuse later
            if (_eventData == null) _eventData = new PointerEventData(EventSystem.current);

            // Always update pointer state
            _eventData.Reset(); // reset internal fields
            _eventData.position = Input.mousePosition;

            _results.Clear();
            EventSystem.current.RaycastAll(_eventData, _results);

            foreach (RaycastResult result in _results)
            {
                Canvas canvas = result.gameObject.GetComponentInParent<Canvas>();
                if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
