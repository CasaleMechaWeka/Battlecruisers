using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public abstract class DefenceBuildingBalancingTestGod : MonoBehaviour
    {
        private ICircularList<Camera> _cameras;

        public Camera overviewCamera;

        protected abstract IPrefabKey UnitKey { get; }
        protected abstract IPrefabKey BasicDefenceBuildingKey { get; }
        protected abstract IPrefabKey AdvancedDefenceBuildingKey { get; }

        private Camera _currentCamera;
        private Camera CurrentCamera
        {
            set
            {
                if (_currentCamera != null)
                {
                    _currentCamera.enabled = false;
                }

                _currentCamera = value;

                if (_currentCamera != null)
                {
                    _currentCamera.enabled = true;
                }
            }
        }

        void Start()
        {
            IList<Camera> cameras = new List<Camera>()
            {
                overviewCamera
            };

            AntiAirBalancingTest[] tests = FindObjectsOfType<AntiAirBalancingTest>();

            IList<AntiAirBalancingTest> orderedTests =
                tests
                    .OrderBy(test => test.gameObject.transform.position.x)
                    .ToList();

            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());

            foreach (AntiAirBalancingTest test in orderedTests)
            {
                test.Initialise(prefabFactory, UnitKey, BasicDefenceBuildingKey, AdvancedDefenceBuildingKey);
                cameras.Add(test.Camera);
            }

            _cameras = new CircularList<Camera>(cameras);

            ToggleCamera();
        }

        public void ToggleCamera()
        {
            CurrentCamera = _cameras.Next();
        }
    }
}
