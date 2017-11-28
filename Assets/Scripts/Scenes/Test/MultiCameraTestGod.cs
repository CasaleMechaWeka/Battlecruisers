using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
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
                    .OrderBy(test => test.gameObject.transform.position.x)
                    .ToList();

            foreach (TTest test in orderedTests)
            {
                InitialiseScenario(test);
                cameras.Add(test.Camera);
            }

            _cameras = new CircularList<Camera>(cameras);

            ToggleCamera();
        }

        protected virtual void Initialise() { }

        protected abstract ITestScenario InitialiseScenario(TTest scenario);

        public void ToggleCamera()
        {
            CurrentCamera = _cameras.Next();
        }
    }
}
