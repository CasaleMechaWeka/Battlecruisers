using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class NavigationTestGod : MonoBehaviour
    {
        private INavigationWheelPanel _navigationWheelPanel;
        private ICameraNavigationWheelCalculator _cameraNavigationWheelCalculator;
        private ICamera _camera;

        private void Start()
        {
            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            _navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel();

            Camera platformCamera = FindObjectOfType<Camera>();
            _camera = new CameraBC(platformCamera);
            ISettingsManager settingsManager = Substitute.For<ISettingsManager>();
            ICameraCalculator cameraCalculator = new CameraCalculator(_camera, settingsManager);

            IRange<float> validOrthographicSizes = new Range<float>(min: 5, max: 33);

            _cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(_navigationWheelPanel, cameraCalculator, validOrthographicSizes);
        }

        private void Update()
        {
            // FLEIX  TEMP
            //float yProportion = _navigationWheelPanel.FindNavigationWheelYPositionAsProportionOfMaxHeight();
            //Debug.Log("yProportion: " + yProportion);

            //float xProportion = _navigationWheelPanel.FindXProportion();
            //Debug.Log("xProportion: " + xProportion);

            float desiredCameraOrthographicSize = _cameraNavigationWheelCalculator.FindOrthographicSize();
            Vector2 desiredCameraPosition = _cameraNavigationWheelCalculator.FindCameraPosition();

            _camera.OrthographicSize = desiredCameraOrthographicSize;
            _camera.Transform.Position = new Vector3(desiredCameraPosition.x, desiredCameraPosition.y, _camera.Transform.Position.z);
        }
    }
}