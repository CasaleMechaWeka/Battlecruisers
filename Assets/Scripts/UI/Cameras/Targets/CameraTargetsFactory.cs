using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets
{
    public class CameraTargetsFactory : ICameraTargetsFactory
    {
		private readonly ICamera _camera;
        private readonly ICameraCalculator _cameraCalculator;
		private readonly ICruiser _playerCruiser, _aiCruiser; 

		private const float MID_VIEWS_ORTHOGRAPHIC_SIZE = 18;
        private const float MID_VIEWS_POSITION_X = 20;

		public CameraTargetsFactory(ICamera camera, ICameraCalculator cameraCalculator, ICruiser playerCruiser, ICruiser aiCruiser)
		{
			Helper.AssertIsNotNull(camera, cameraCalculator, playerCruiser, aiCruiser);

			_camera = camera;
			_cameraCalculator = cameraCalculator;
			_playerCruiser = playerCruiser;
			_aiCruiser = aiCruiser;
        }

        public IDictionary<CameraState, ICameraTargetLegacy> CreateCameraTargets()
        {
			IDictionary<CameraState, ICameraTargetLegacy> stateToTarget = new Dictionary<CameraState, ICameraTargetLegacy>();

			// Overview.  Camera starts in overiview (ish, y-position is only roughly right :P)
			Vector3 overviewTargetPosition = _camera.Transform.Position;
			overviewTargetPosition.y = _cameraCalculator.FindCameraYPosition(_camera.OrthographicSize);
			stateToTarget.Add(CameraState.Overview, new CameraTargetLegacy(overviewTargetPosition, _camera.OrthographicSize, CameraState.Overview));
			
			// Player cruiser view
			float playerCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(_playerCruiser);
			Vector3 playerCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(_playerCruiser, playerCruiserOrthographicSize, _camera.Transform.Position.z);
			CameraState[] leftSideInstants =
			{
				CameraState.RightMid,
				CameraState.AiCruiser
			};
			ICameraTargetLegacy playerCruiserTarget
    			= new CameraTargetLegacy(
    				playerCruiserTargetPosition,
    				playerCruiserOrthographicSize,
    				CameraState.PlayerCruiser,
    				leftSideInstants);
			stateToTarget.Add(CameraState.PlayerCruiser, playerCruiserTarget);
			
			// Ai cruiser overview
			float aiCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(_aiCruiser);
			Vector3 aiCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(_aiCruiser, aiCruiserOrthographicSize, _camera.Transform.Position.z);
			CameraState[] rightSideInstants =
			{
				CameraState.LeftMid,
				CameraState.PlayerCruiser
			};
			ICameraTargetLegacy aiCruiserTarget
    			= new CameraTargetLegacy(
    				aiCruiserTargetPosition,
    				aiCruiserOrthographicSize,
    				CameraState.AiCruiser,
				    rightSideInstants);
			stateToTarget.Add(CameraState.AiCruiser, aiCruiserTarget);
			
			float midViewsPositionY = _cameraCalculator.FindCameraYPosition(MID_VIEWS_ORTHOGRAPHIC_SIZE);
			
			// Left mid view
			Vector3 leftMidViewPosition = new Vector3(-MID_VIEWS_POSITION_X, midViewsPositionY, _camera.Transform.Position.z);
			stateToTarget.Add(CameraState.LeftMid, new CameraTargetLegacy(leftMidViewPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.LeftMid, leftSideInstants));
			
			// Right mid view
			Vector3 rightMidPosition = new Vector3(MID_VIEWS_POSITION_X, midViewsPositionY, _camera.Transform.Position.z);
			stateToTarget.Add(CameraState.RightMid, new CameraTargetLegacy(rightMidPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.RightMid, rightSideInstants));

			return stateToTarget;
		}
    }
}
