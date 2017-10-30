using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MissileLauncherBarrelWrapper : StaticBarrelWrapper
    {
        private float _desiredAngleInDegrees;
        protected override float DesiredAngleInDegrees { get { return _desiredAngleInDegrees; } }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Assert.IsTrue(_barrels.Length != 0);
			_desiredAngleInDegrees = _barrels[0].transform.eulerAngles.z;
        }

        protected override IRotationMovementController CreateRotationMovementController(BarrelController barrel)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }
	}
}
