using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class HighlightingTestGod : MonoBehaviour
    {
        public Camera camera;

        void Start()
        {

        }

        // Move camera.  Highlights should stay in place correctly on both
        // the in game and on canvas objects.
        void Update()
        {
            Vector3 currentPosition = camera.transform.position;
            float xPosition = currentPosition.x + Time.deltaTime;
            camera.transform.position = new Vector3(xPosition, currentPosition.y, currentPosition.z);
        }
    }
}
