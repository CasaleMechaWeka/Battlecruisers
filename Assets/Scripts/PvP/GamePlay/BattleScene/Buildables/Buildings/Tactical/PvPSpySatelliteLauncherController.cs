using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPSpySatelliteLauncherController : PvPSatelliteLauncherController, IPvPBuilding
    {
        protected override Vector3 SpawnPositionAdjustment => new Vector3(0, 0.17f, 0);
        public override TargetValue TargetValue => TargetValue.Medium;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            Assert.IsNotNull(satellitePrefab);
            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.StaticInitialise();
        }
    }
}