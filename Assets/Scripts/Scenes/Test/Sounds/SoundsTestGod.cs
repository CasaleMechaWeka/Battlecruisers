using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Sounds
{
    // Old plan???
    // 1. Add zooming, see if volume changes
    // 2. Add music, to have base volume
    // 3. Other sound effects?  Explosion???
    public class SoundsTestGod : TestGodBase
    {
        private TestAircraftController _aircraft;
        public List<Vector2> patrolPoints;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: Utils.BuildSpeedMultipliers.DEFAULT, updaterProvider: updaterProvider);
        }

        protected override List<GameObject> GetGameObjects()
        {
            _aircraft = FindObjectOfType<TestAircraftController>();

            return new List<GameObject>()
            {
                _aircraft.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            _aircraft.PatrolPoints = patrolPoints;
            helper.InitialiseUnit(_aircraft);
            _aircraft.StartConstruction();
        }
    }
}
