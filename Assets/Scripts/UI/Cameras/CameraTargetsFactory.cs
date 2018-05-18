using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
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

        public IDictionary<CameraState, ICameraTarget> CreateCameraTargets()
        {
			IDictionary<CameraState, ICameraTarget> stateToTarget = new Dictionary<CameraState, ICameraTarget>();

			// Overview.  Camera starts in overiview (ish, y-position is only roughly right :P)
			Vector3 overviewTargetPosition = _camera.Position;
			overviewTargetPosition.y = _cameraCalculator.FindCameraYPosition(_camera.OrthographicSize);
			IList<CameraState> overviewInstants = new List<CameraState>();
			stateToTarget.Add(CameraState.Overview, new CameraTarget(overviewTargetPosition, _camera.OrthographicSize, CameraState.Overview, overviewInstants));
			
			// Player cruiser view
			float playerCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(_playerCruiser);
			Vector3 playerCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(_playerCruiser, playerCruiserOrthographicSize, _camera.Position.z);
			IList<CameraState> leftSideInstants = new List<CameraState>
			{
				CameraState.RightMid,
				CameraState.AiCruiser
			};
			stateToTarget.Add(CameraState.PlayerCruiser, new CameraTarget(playerCruiserTargetPosition, playerCruiserOrthographicSize, CameraState.PlayerCruiser, leftSideInstants));
			
			// Ai cruiser overview
			float aiCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(_aiCruiser);
			Vector3 aiCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(_aiCruiser, aiCruiserOrthographicSize, _camera.Position.z);
			IList<CameraState> rightSideInstants = new List<CameraState>
			{
				CameraState.LeftMid,
				CameraState.PlayerCruiser
			};
			stateToTarget.Add(CameraState.AiCruiser, new CameraTarget(aiCruiserTargetPosition, aiCruiserOrthographicSize, CameraState.AiCruiser, rightSideInstants));
			
			float midViewsPositionY = _cameraCalculator.FindCameraYPosition(MID_VIEWS_ORTHOGRAPHIC_SIZE);
			
			// Left mid view
			Vector3 leftMidViewPosition = new Vector3(-MID_VIEWS_POSITION_X, midViewsPositionY, _camera.Position.z);
			stateToTarget.Add(CameraState.LeftMid, new CameraTarget(leftMidViewPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.LeftMid, leftSideInstants));
			
			// Right mid view
			Vector3 rightMidPosition = new Vector3(MID_VIEWS_POSITION_X, midViewsPositionY, _camera.Position.z);
			stateToTarget.Add(CameraState.RightMid, new CameraTarget(rightMidPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.RightMid, rightSideInstants));

			return stateToTarget;
		}
    }
}
