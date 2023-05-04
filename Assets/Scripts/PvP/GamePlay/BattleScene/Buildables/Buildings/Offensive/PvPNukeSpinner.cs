using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive
{
    public class PvPNukeSpinner : MonoBehaviour
    {
        private IPvPConstantRotationController _activeRotationController, _constantRotationController, _dummyRotationController;

        public float rotateSpeedInDegreesPerS;

        public SpriteRenderer Renderer { get; private set; }

        public void StaticInitialise()
        {
            Renderer = gameObject.GetComponent<SpriteRenderer>();
            Assert.IsNotNull(Renderer);

            Assert.IsTrue(rotateSpeedInDegreesPerS > 0);
        }

        public void Initialise(IPvPMovementControllerFactory movementControllerFactory)
        {
            _constantRotationController = movementControllerFactory.CreateConstantRotationController(rotateSpeedInDegreesPerS, transform);
            _dummyRotationController = movementControllerFactory.CreateDummyConstantRotationController();
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
