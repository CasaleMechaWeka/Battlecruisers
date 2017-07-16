using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Movement.Rotation;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class NukeSpinner : MonoBehaviour
	{
		private IConstantRotationController _rotationController;

		public SpriteRenderer Renderer { get; private set; }

		public void StaticInitialise()
		{
			Renderer = gameObject.GetComponent<SpriteRenderer>();
			Assert.IsNotNull(Renderer);
		}

		public void Initialise(IConstantRotationController rotationController)
		{
			_rotationController = rotationController;
		}

		void FixedUpdate()
		{
			Debug.Log("FixedUpdate()  _rotationController: " + _rotationController);

			_rotationController.Rotate();
		}
	}
}
