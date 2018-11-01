using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
    // FELIX  Remove transitioning camera :)
    public class CameraTargetLegacy : ICameraTargetLegacy
	{
		private readonly IList<CameraState> _instantStates;

		public Vector3 Position { get; private set; } 
		public float OrthographicSize { get; private set; }
		public CameraState State { get; private set; }

		public CameraTargetLegacy(Vector3 position, float orthographicSize, CameraState state, params CameraState[] instantStates)
		{
			Assert.IsNotNull(instantStates);

			Position = position;
			OrthographicSize = orthographicSize;
			State = state;
			_instantStates = instantStates;
		}

		public bool IsInstantTransition(CameraState fromState)
		{
			return _instantStates.Contains(fromState);
		}
	}
}
