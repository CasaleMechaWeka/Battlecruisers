using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class HighlightingTestGod : MonoBehaviour
    {
        public Camera camera;
        public GameObject inGameObject, onCanvasObject;
        public GameObject inGameHighlightPrefab, onCanvasHighlightPrefab;

        void Start()
        {
            Instantiate(onCanvasHighlightPrefab, onCanvasObject.transform);
            Instantiate(inGameHighlightPrefab, inGameObject.transform);
        }

        void Update()
        {
            MoveCamera();
        }

        /// <summary>
		/// Moves camera.  Highlights should stay in place correctly on both
		/// the in game and on canvas objects.
        /// </summary>
        private void MoveCamera()
        {
            Vector3 currentPosition = camera.transform.position;
            float xPosition = currentPosition.x + Time.deltaTime;
            camera.transform.position = new Vector3(xPosition, currentPosition.y, currentPosition.z);
        }
    }
}
