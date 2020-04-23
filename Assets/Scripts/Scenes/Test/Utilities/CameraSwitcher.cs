using UnityEngine;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class CameraSwitcher
    {
        private Camera _activeCamera;
        public Camera ActiveCamera
        {
            set
            {
                if (_activeCamera != null)
                {
                    // FELIX
                    _activeCamera.gameObject.SetActive(false);
                    //_activeCamera.enabled = false;
                }

                _activeCamera = value;

                if (_activeCamera != null)
                {
                    // FELIX
                    _activeCamera.gameObject.SetActive(true);
                    //_activeCamera.enabled = true;
                }
            }
        }
    }
}