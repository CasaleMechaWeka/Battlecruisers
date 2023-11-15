using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPStationaryBarrelWrapper : PvPStaticBarrelWrapper
    {
        private float _desiredAngleInDegrees;
        protected override float DesiredAngleInDegrees => _desiredAngleInDegrees;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Assert.IsTrue(_barrels.Length != 0);
            _desiredAngleInDegrees = _barrels[0].transform.eulerAngles.z;
        }

        protected override IPvPRotationMovementController CreateRotationMovementController(IPvPBarrelController barrel, IPvPDeltaTimeProvider deltaTimeProvider)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }
    }
}
