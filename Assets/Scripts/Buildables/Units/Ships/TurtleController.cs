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
            _shieldController.gameObject.SetActive(false);
            base.StaticInitialise(parent, healthBar);
            _shieldController.StaticInitialise();
        }

        protected override void OnBuildableCompleted()
        {
            _shieldController.Initialise(Faction, TargetType.Ships);
            base.OnBuildableCompleted();
            _shieldController.gameObject.SetActive(true);
        }
    }
}
