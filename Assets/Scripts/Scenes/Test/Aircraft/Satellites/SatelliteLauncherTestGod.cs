using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Offensive;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class SatelliteLauncherTestGod : CameraToggleTestGod
    {
        public bool useFastBuildSpeed;

        private SatelliteLauncherController[] _launchers;

        protected override IList<GameObject> GetGameObjects()
        {
            _launchers = FindObjectsOfType<SatelliteLauncherController>();
            return
                _launchers
                .Select(launcher => launcher.GameObject)
                .ToList();
        }

        protected override void Setup(Helper helper)
        {
            foreach (SatelliteLauncherController launcher in _launchers)
            {
                Vector2 parentCruiserPosition = launcher.transform.position;
                Vector2 enemyCruiserPosition = new Vector2(launcher.transform.position.x + 30, launcher.transform.position.y);
                IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BCUtils.RandomGenerator.Instance);

                helper.InitialiseBuilding(launcher, aircraftProvider: aircraftProvider);
                launcher.StartConstruction();
            }
        }

        protected async override Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            float buildSpeedMultiplier = useFastBuildSpeed ? BCUtils.BuildSpeedMultipliers.FAST : BCUtils.BuildSpeedMultipliers.DEFAULT;
            return await HelperFactory.CreateHelperAsync(updaterProvider: updaterProvider, buildSpeedMultiplier: buildSpeedMultiplier);
        }

        public void DestroyLaunchers()
        {
            foreach (SatelliteLauncherController launcher in _launchers)
            {
                launcher.Destroy();
            }
        }
    }
}
