using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class TurtleController : ShipController
    {
        private SectorShieldController _shieldController;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            _shieldController = GetComponentInChildren<SectorShieldController>(includeInactive: true);
            UnityEngine.Assertions.Assert.IsNotNull(_shieldController, $"{name} is missing a SectorShieldController child (required by TurtleController).");
            _shieldController.gameObject.SetActive(false);
            base.StaticInitialise(parent, healthBar);
            _shieldController.StaticInitialise();
        }

        protected override void OnBuildableCompleted()
        {
            UnityEngine.Assertions.Assert.IsNotNull(_shieldController, $"{name} _shieldController is null in OnBuildableCompleted().");

            // Pool safety: the shield target can persist between activations and may be depleted.
            // Ensure it's in a valid state before wiring its healthbar & enabling visuals/collider.
            if (_shieldController != null && _shieldController.MaxHealth > 0 && _shieldController.Health <= 0)
            {
                _shieldController.SetHealthToMax();
            }

            _shieldController.Initialise(Faction, TargetType.Ships);
            base.OnBuildableCompleted();
            _shieldController.gameObject.SetActive(true);
        }
    }
}
