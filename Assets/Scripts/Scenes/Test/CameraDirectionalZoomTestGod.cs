using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class CameraDirectionalZoomTestGod : MonoBehaviour
    {
        public Camera camera;
        public GameObject zoomTarget;

        private void Start()
        {
            //Debug.Log("camera.transform.position: " + camera.transform.position);

            Vector3 zoomTargetViewportPosition = camera.WorldToViewportPoint(zoomTarget.transform.position);
            Debug.Log("zoomTargetViewportPosition: " + zoomTargetViewportPosition);
            Debug.Log("Camera size: " + FindSize());

            camera.orthographicSize = 3;
            camera.transform.position = new Vector3(-2.6f, 0.6f, camera.transform.position.z);

            //Debug.Log("camera.transform.position: " + camera.transform.position);

            zoomTargetViewportPosition = camera.WorldToViewportPoint(zoomTarget.transform.position);
            Debug.Log("zoomTargetViewportPosition: " + zoomTargetViewportPosition);
            Debug.Log("Camera size: " + FindSize());
        }

        private Vector2 FindSize()
        {
            float height = 2 * camera.orthographicSize;
            float width = height * camera.aspect;
            return new Vector2(width, height);
        }
    }
}