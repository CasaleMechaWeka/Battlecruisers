using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public abstract class MultiCameraTestGod<TTest> : MonoBehaviour where TTest : MonoBehaviour, ITestScenario
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
            Initialise();

            IList<Camera> cameras = new List<Camera>()
            {
                overviewCamera
            };

            TTest[] tests = FindObjectsOfType<TTest>();

            IList<TTest> orderedTests =
                tests
                    .OrderBy(test => OrderBy(test))
                    .ToList();

            foreach (TTest test in orderedTests)
            {
                InitialiseScenario(test);
                cameras.Add(test.Camera);
                test.Camera.enabled = false;
            }

            _cameras = new CircularList<Camera>(cameras);

            ToggleCamera();
        }

        protected virtual void Initialise() { }

        protected abstract void InitialiseScenario(TTest scenario);

        protected virtual float OrderBy(TTest scenario)
        {
            return scenario.gameObject.transform.position.x;
        }

        public void ToggleCamera()
        {
            CurrentCamera = _cameras.Next();
        }
    }
}
