using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface IInput
	{
		Vector3 MousePosition { get; }
		Vector2 MouseScrollDelta { get; }
		int TouchCount { get; }

		Vector2 GetTouchPosition(int touchIndex);
	}
}
