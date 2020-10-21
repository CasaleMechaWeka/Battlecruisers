using System;
using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class InputBC : IInput
    {
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
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
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
