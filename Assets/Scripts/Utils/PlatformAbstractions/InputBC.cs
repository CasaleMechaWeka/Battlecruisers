using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class InputBC : IInput
    {
		public Vector3 MousePosition => Input.mousePosition;
		public Vector2 MouseScrollDelta => Input.mouseScrollDelta;
		public int TouchCount => Input.touchCount;

		public Vector2 GetTouchPosition(int touchIndex)
		{
			return Input.touches[touchIndex].position;
		}
	}
}
