using UnityEngine;
using System.Collections.Generic;

namespace Asce.Managers.Utils
{
    public static class KeyCodeUtils
    {
        private static readonly Dictionary<KeyCode, string> _keyNames = new Dictionary<KeyCode, string>()
        {
            { KeyCode.Alpha0, "0" },
            { KeyCode.Alpha1, "1" },
            { KeyCode.Alpha2, "2" },
            { KeyCode.Alpha3, "3" },
            { KeyCode.Alpha4, "4" },
            { KeyCode.Alpha5, "5" },
            { KeyCode.Alpha6, "6" },
            { KeyCode.Alpha7, "7" },
            { KeyCode.Alpha8, "8" },
            { KeyCode.Alpha9, "9" },
            { KeyCode.Keypad0, "Num 0" },
            { KeyCode.Keypad1, "Num 1" },
            { KeyCode.Keypad2, "Num 2" },
            { KeyCode.Keypad3, "Num 3" },
            { KeyCode.Keypad4, "Num 4" },
            { KeyCode.Keypad5, "Num 5" },
            { KeyCode.Keypad6, "Num 6" },
            { KeyCode.Keypad7, "Num 7" },
            { KeyCode.Keypad8, "Num 8" },
            { KeyCode.Keypad9, "Num 9" },
            { KeyCode.Return, "Enter" },
            { KeyCode.LeftShift, "Shift" },
            { KeyCode.RightShift, "Shift" },
            { KeyCode.LeftControl, "Ctrl" },
            { KeyCode.RightControl, "Ctrl" },
            { KeyCode.LeftAlt, "Alt" },
            { KeyCode.RightAlt, "Alt" },
            { KeyCode.Mouse0, "LMB" },
            { KeyCode.Mouse1, "RMB" },
            { KeyCode.Mouse2, "MMB" },
        };

        public static string ToReadableString(this KeyCode keyCode)
        {
            if (_keyNames.TryGetValue(keyCode, out string name))
                return name;

            string keyName = keyCode.ToString();

            // Remove prefixes like "Alpha", "Keypad"
            if (keyName.StartsWith("Alpha"))
                keyName = keyName.Substring(5);
            else if (keyName.StartsWith("Keypad"))
                keyName = "Num " + keyName.Substring(6);

            return keyName;
        }
    }
}