using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class InputBC : IInput
    {
        private static IList<KeyCode> ValidHotkeys = new List<KeyCode>()
        {
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.I,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.M,
            KeyCode.N,
            KeyCode.O,
            KeyCode.P,
            KeyCode.Q,
            KeyCode.R,
            KeyCode.S,
            KeyCode.T,
            KeyCode.U,
            KeyCode.V,
            KeyCode.W,
            KeyCode.X,
            KeyCode.Y,
            KeyCode.Z,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.RightArrow,
            KeyCode.LeftArrow,
            KeyCode.Insert,
            KeyCode.Home,
            KeyCode.End,
            KeyCode.PageUp,
            KeyCode.PageDown,
            KeyCode.Backspace,
            KeyCode.Tab,
            KeyCode.Clear,
            KeyCode.Return,
            KeyCode.Pause,
            KeyCode.Space,
            KeyCode.Exclaim,
            KeyCode.DoubleQuote,
            KeyCode.Hash,
            KeyCode.Dollar,
            KeyCode.Percent,
            KeyCode.Ampersand,
            KeyCode.Quote,
            KeyCode.LeftParen,
            KeyCode.RightParen,
            KeyCode.Asterisk,
            KeyCode.Plus,
            KeyCode.Comma,
            KeyCode.Minus,
            KeyCode.Period,
            KeyCode.Slash,
            KeyCode.Semicolon,
            KeyCode.Less,
            KeyCode.Equals,
            KeyCode.Greater,
            KeyCode.Question,
            KeyCode.At,
            KeyCode.LeftBracket,
            KeyCode.Backslash,
            KeyCode.RightBracket,
            KeyCode.Caret,
            KeyCode.Underscore,
            KeyCode.BackQuote,
            KeyCode.F1,
            KeyCode.F2,
            KeyCode.F3,
            KeyCode.F4,
            KeyCode.F5,
            KeyCode.F6,
            KeyCode.F7,
            KeyCode.F8,
            KeyCode.F9,
            KeyCode.F10,
            KeyCode.F11,
            KeyCode.F12,
            KeyCode.F13,
            KeyCode.F14,
            KeyCode.F15
        };

		public Vector3 MousePosition => Input.mousePosition;
		public Vector2 MouseScrollDelta => Input.mouseScrollDelta;
		public int TouchCount => Input.touchCount;

		private static IInput _instance;
		public static IInput Instance
        {
			get
            {
				if (_instance == null)
                {
					_instance = new InputBC();
                }
				return _instance;
            }
        }

		private InputBC() { }

        public Vector2 GetTouchPosition(int touchIndex)
		{
			return Input.touches[touchIndex].position;
		}

        public bool GetKeyUp(KeyCode key)
        {
			return Input.GetKeyUp(key);
        }

        public KeyCode GetFirstKeyDown()
        {
            foreach (KeyCode key in ValidHotkeys)
            {
                if (Input.GetKeyDown(key))
                {
                    return key;
                }
            }

            return KeyCode.None;
        }
    }
}
