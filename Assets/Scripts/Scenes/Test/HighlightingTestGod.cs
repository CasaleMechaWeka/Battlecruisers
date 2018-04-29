using BattleCruisers.Tutorial;
using BattleCruisers.Utils.Threading;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class HighlightingTestGod : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject inGameObject, onCanvasObject;
        public InGameHighlight inGameHighlightPrefab;
        //, onCanvasHighlightPrefab;
        public ConstDelayDeferrer deferrer;

        void Start()
        {
            deferrer.StaticInitialise(delayInMs: 2000);

            //Instantiate(onCanvasHighlightPrefab, onCanvasObject.transform);

            SpriteRenderer renderer = inGameObject.GetComponent<SpriteRenderer>();
            float radius = renderer.size.x / 2;

            IHighlight inGameHighlight = Instantiate(inGameHighlightPrefab);
            inGameHighlight.Initialise(radius, inGameObject.transform.position);

            deferrer.Defer(() => inGameHighlight.Destroy());
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
            Vector3 currentPosition = mainCamera.transform.position;
            float xPosition = currentPosition.x + Time.deltaTime;
            mainCamera.transform.position = new Vector3(xPosition, currentPosition.y, currentPosition.z);
        }
    }
}
