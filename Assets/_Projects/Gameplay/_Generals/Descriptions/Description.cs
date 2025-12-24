using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game
{
    [System.Serializable]
    public class Description
    {
        [SerializeField, TextArea(3, 10)] private string _text;
        [SerializeField, TextArea(2, 5)] private string _sideNote;

        public string RawText
        {
            get => _text;
            set => _text = value;
        }

        public string SideNote
        {
            get => _sideNote;
            set => _sideNote = value;
        }

        public string GetDescription(Dictionary<string, string> replaceContents)
        {
            if (string.IsNullOrEmpty(_text)) return string.Empty;
            List<string> keys = _text.ExtractKeys();

            string description = _text;
            foreach (string key in keys)
            {
                if (replaceContents.TryGetValue(key, out string value))
                {
                    description = description.Replace($"{{{key}}}", value);
                }
            }

            return description;
        }

        public string GetSideNote(Dictionary<string, string> replaceContents)
        {
            if (string.IsNullOrEmpty(_sideNote)) return string.Empty;
            List<string> keys = _sideNote.ExtractKeys();

            string sideNote = _sideNote;
            foreach (string key in keys)
            {
                if (replaceContents.TryGetValue(key, out string value))
                {
                    sideNote = sideNote.Replace($"{{{key}}}", value);
                }
            }
            return sideNote;
        }
    }
}