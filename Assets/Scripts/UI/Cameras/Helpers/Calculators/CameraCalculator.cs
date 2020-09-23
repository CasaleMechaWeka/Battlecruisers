using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public class CameraCalculator : ICameraCalculator
	{
		private readonly ICamera _camera;
        private readonly ICameraCalculatorSettings _settings;

		public CameraCalculator(ICamera camera, ICameraCalculatorSettings settings)
		{
            Helper.AssertIsNotNull(camera, settings);

			_camera = camera;
            _settings = settings;
		}

		// height = 2 * orthographic size
		// width = height * aspect ratio
		public float FindCameraOrthographicSize(ICruiser cruiser)
		{
			float desiredWidth = cruiser.Size.x * _settings.CruiserWidthMultiplier;
			float desiredHeight = desiredWidth / _camera.Aspect;
			float desiredOrthographicSize = desiredHeight / 2;

			if (desiredOrthographicSize < _settings.ValidOrthographicSizes.Min)
			{
				desiredOrthographicSize = _settings.ValidOrthographicSizes.Min;
			}

			return desiredOrthographicSize;
		}

		public float FindCameraYPosition(float desiredOrthographicSize)
		{
			float desiredHeight = 2 * desiredOrthographicSize;
			return desiredOrthographicSize + _settings.MaxWaterPositionY - (_settings.WaterProportion * desiredHeight);
		}

        public float FindScrollSpeed(float orthographicSize, float timeDelta)
        {
			float scrollSpeedPerS = _settings.ScrollSpeedGradient * orthographicSize + _settings.ScrollSpeedConstant;
			return scrollSpeedPerS * timeDelta * _settings.ScrollSpeed;
        }

        public Vector3 FindCruiserCameraPosition(ICruiser cruiser, float orthographicSize, float zValue)
        {
            // Want the cruiser camera to be slightly off centre, towards the 
            // front of the cruiser.  That way the naval factory and the ship
            // under construction are easily visible.
            float xAdjustmentMagnitudeInM = cruiser.Size.x * _settings.CruiserCameraPositionAdjustmentMultiplier;
            float xAdjustmentInM = cruiser.IsPlayerCruiser ? xAdjustmentMagnitudeInM : -xAdjustmentMagnitudeInM;
            
            Vector3 cameraPosition
                = new Vector3(
                    cruiser.Position.x + xAdjustmentInM, 
                    FindCameraYPosition(orthographicSize), 
                    zValue);

            Logging.Log(Tags.CAMERA_CALCULATOR, $"Cruiser position: {cruiser.Position}  Camera position: {cameraPosition}");

            return cameraPosition;
        }

        public Vector3 FindZoomingCameraPosition(
            Vector2 zoomTarget, 
            Vector2 targetViewportPosition, 
            float newCameraOrthographicSize, 
            float cameraAspectRatio,
            float cameraPositionZ)
        {
            float cameraHeight = 2 * newCameraOrthographicSize;
            float cameraWidth = cameraAspectRatio * cameraHeight;

            return
                new Vector3(
                    FindDimension(cameraWidth, zoomTarget.x, targetViewportPosition.x),
                    FindDimension(cameraHeight, zoomTarget.y, targetViewportPosition.y),
                    cameraPositionZ);
        }

        private float FindDimension(float dimension, float zoomTargetDimension, float targetViewportDimension)
        {
            return (dimension / 2) + zoomTargetDimension - (targetViewportDimension * dimension);
        }

        public IRange<float> FindValidCameraXPositions(float desiredOrthographicSize)
        {
            Assert.IsTrue(desiredOrthographicSize >= _settings.ValidOrthographicSizes.Min, $"Othographic size should be >= {_settings.ValidOrthographicSizes.Min}, but is: {desiredOrthographicSize}");
            Assert.IsTrue(desiredOrthographicSize <= _settings.ValidOrthographicSizes.Max, $"Othographic size should be <= {_settings.ValidOrthographicSizes.Max}, but is: {desiredOrthographicSize}");

            float cameraHeight = 2 * desiredOrthographicSize;
            float cameraWidth = _camera.Aspect * cameraHeight;
            float halfCameraWidth = cameraWidth / 2;

            float minValidX = _settings.CameraVisibleXRange.Min + halfCameraWidth;
            Assert.IsTrue(minValidX <= 0);

            float maxValidX = _settings.CameraVisibleXRange.Max - halfCameraWidth;
            Assert.IsTrue(maxValidX >= 0);

            return new Range<float>(minValidX, maxValidX);
        }
    }
}
