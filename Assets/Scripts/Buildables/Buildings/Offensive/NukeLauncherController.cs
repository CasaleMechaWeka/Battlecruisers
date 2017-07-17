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

		public RotatingController leftSiloHalf, rightSiloHalf;

		private const float SILO_HALVES_ROTATE_SPEED_IN_M_PER_S = 15;
		private const float LEFT_SILO_TARGET_ANGLE_IN_DEGREES = 45;
		private const float RIGHT_SILO_TARGET_ANGLE_IN_DEGREES = 315;

		public override TargetValue TargetValue { get { return TargetValue.High; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			// FELIX
			Assert.IsNotNull(leftSiloHalf);
//			Assert.IsNotNull(rightSiloHalf);

			_spinner = gameObject.GetComponentInChildren<NukeSpinner>();
			Assert.IsNotNull(_spinner);
			_spinner.StaticInitialise();
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			leftSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, LEFT_SILO_TARGET_ANGLE_IN_DEGREES);

			_spinner.Initialise(_movementControllerFactory);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			// FELIX  Spinner is never visible :(
			_spinner.StartRotating();
			_spinner.StopRotating();
			_spinner.Renderer.enabled = false;

			leftSiloHalf.ReachedDesiredAngle += SiloHalf_ReachedDesiredAngle;

			leftSiloHalf.StartRotating();
		}

		private void SiloHalf_ReachedDesiredAngle(object sender, EventArgs e)
		{
			leftSiloHalf.ReachedDesiredAngle -= SiloHalf_ReachedDesiredAngle;

			// FELIX  Launch rocket!
		}

		protected override void EnableRenderers(bool enabled)
		{
			base.EnableRenderers(enabled);

			_spinner.Renderer.enabled = enabled;
		}
	}
}
