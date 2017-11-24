using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    // FELIX  Avoid duplicate code with AntiAirBalancingTestGod :)
    public class AntiAirBalancingTestGod : MonoBehaviour
    {
        private ICircularList<Camera> _cameras;

        public Camera overviewCamera;

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
            IPrefabKey bomberKey = StaticPrefabKeys.Units.Bomber;
            IPrefabKey antiAirKey = StaticPrefabKeys.Buildings.AntiAirTurret;
            IPrefabKey samSiteKey = StaticPrefabKeys.Buildings.SamSite;

            foreach (AntiAirBalancingTest test in orderedTests)
            {
                test.Initialise(prefabFactory, bomberKey, antiAirKey, samSiteKey);
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