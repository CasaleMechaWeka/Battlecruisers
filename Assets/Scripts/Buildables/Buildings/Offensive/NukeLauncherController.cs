using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class NukeLauncherController : Building
	{
		private NukeSpinner _spinner;

		private const float SPINNER_ROTATE_SPEED_IN_M_DEGREES_S = 45;

		public override TargetValue TargetValue { get { return TargetValue.High; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_spinner = gameObject.GetComponentInChildren<NukeSpinner>();
			Assert.IsNotNull(_spinner);
			_spinner.StaticInitialise();
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_spinner.Initialise(_movementControllerFactory.CreateConstantRotationController(SPINNER_ROTATE_SPEED_IN_M_DEGREES_S, _spinner.transform));
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			// FELIX  Open & launch nuke :D
		}

		protected override void EnableRenderers(bool enabled)
		{
			base.EnableRenderers(enabled);

			_spinner.Renderer.enabled = enabled;
		}
	}
}
