using UnityEngine;

namespace Asce.Game.UIs
{
    public static class UIHighlightEventSystem
    {
        public static void Highlight(this IUIHighlightable highlight)
        {
            if (highlight == null) return;
            UIHighlightGroup group = highlight.RectTransform.GetComponentInParent<UIHighlightGroup>();
            if (group != null)
            {
                group.Set(highlight);
            }
        }
    }
}