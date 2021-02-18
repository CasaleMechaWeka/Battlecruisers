using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MissileLauncherBarrelWrapper : StaticBarrelWrapper
    {
        private float _desiredAngleInDegrees;
        protected override float DesiredAngleInDegrees => _desiredAngleInDegrees;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Assert.IsTrue(_barrels.Length != 0);
			_desiredAngleInDegrees = _barrels[0].transform.eulerAngles.z;
        }

        protected override IRotationMovementController CreateRotationMovementController(IBarrelController barrel, IDeltaTimeProvider deltaTimeProvider)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }
	}
}
