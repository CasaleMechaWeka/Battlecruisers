using UnityEngine;
using System.Collections;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    // FELIX  Not MB, extend ICameraAdjuster, make testable :)
    public class ParallaxAdjuster : MonoBehaviour
    {
        public Camera mainCamera;
        public Camera farCamera;
        //public Camera nearCamera;

        void Start()
        {

        }

        void Update()
        {
            // orthoSize
            float orthoSize = mainCamera.orthographicSize;
            // distanceFromOrigin
            float mainCameraZPosition = Mathf.Abs(mainCamera.transform.position.z);

            //change clipping planes based on main camera z-position
            farCamera.nearClipPlane = mainCameraZPosition;
            farCamera.farClipPlane = mainCamera.farClipPlane;
            //nearCamera.farClipPlane = mainCameraZPosition;
            //nearCamera.nearClipPlane = mainCamera.nearClipPlane;

            //update field fo view for parallax cameras
            float fieldOfView = Mathf.Atan(orthoSize / mainCameraZPosition) * Mathf.Rad2Deg * 2f;
            farCamera.fieldOfView = fieldOfView;
            //nearCamera.fieldOfView = farCamera.fieldOfView = fieldOfView;

            Debug.Log($"Main ortho size: {orthoSize}  Field of view: {fieldOfView}");
        }
    }
}