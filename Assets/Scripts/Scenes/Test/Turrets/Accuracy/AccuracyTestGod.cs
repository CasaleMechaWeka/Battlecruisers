using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.Accuracy
{
    public class AccuracyTestGod : MonoBehaviour
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
            
			AccuracyTest[] tests = FindObjectsOfType<AccuracyTest>();

            IList<AccuracyTest> orderedTests =
                tests
                    .OrderBy(test => test.gameObject.GetComponentInChildren<ITurretStats>().Accuracy)
                    .ToList();

            foreach (AccuracyTest test in orderedTests)
            {
                test.Initialise();
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