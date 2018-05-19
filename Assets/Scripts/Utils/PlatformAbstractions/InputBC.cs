using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class InputBC : IInput
    {
		public Vector3 MousePosition { get { return Input.mousePosition; } }
		public Vector2 MouseScrollDelta { get { return Input.mouseScrollDelta; } }
    }
}
