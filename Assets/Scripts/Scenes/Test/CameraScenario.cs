using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    /// <summary>
    /// Just wraps a camera.
    /// </summary>
    public class CameraScenario : MonoBehaviour, ITestScenario
    {
        public Camera Camera { get; private set; }

        public void Initialise()
        {
            Camera = GetComponent<Camera>();
            Assert.IsNotNull(Camera);
        }
    }
}