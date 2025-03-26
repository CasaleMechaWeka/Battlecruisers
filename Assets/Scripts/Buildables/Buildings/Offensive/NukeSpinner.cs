using BattleCruisers.Movement.Rotation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class NukeSpinner : MonoBehaviour
	{
		private IConstantRotationController _activeRotationController, _constantRotationController, _dummyRotationController;

		public float rotateSpeedInDegreesPerS;

		public SpriteRenderer Renderer { get; private set; }

		public void StaticInitialise()
		{
			Renderer = gameObject.GetComponent<SpriteRenderer>();
			Assert.IsNotNull(Renderer);

			Assert.IsTrue(rotateSpeedInDegreesPerS > 0);
		}

		public void Initialise()
		{
			_constantRotationController = new ConstantRotationController(rotateSpeedInDegreesPerS, transform);
			_dummyRotationController = new DummyConstantRotationController();
			_activeRotationController = _dummyRotationController;
		}

		void FixedUpdate()
		{
			_activeRotationController.Rotate();
		}

		public void StartRotating()
		{
			_activeRotationController = _constantRotationController;
		}

		public void StopRotating()
		{
			_activeRotationController = _dummyRotationController;
		}
	}
}
