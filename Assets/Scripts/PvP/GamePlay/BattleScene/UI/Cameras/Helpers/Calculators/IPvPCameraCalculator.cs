using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators
{
    public interface IPvPCameraCalculator
    {
        float FindCameraOrthographicSize(IPvPCruiser cruiser);
        float FindScrollSpeed(float orthographicSize, float timeDelta);
        Vector3 FindCruiserCameraPosition(IPvPCruiser cruiser, float orthographicSize, float zValue);

        /// <summary>
        /// The water should always take up the same proportion of the screen.  Hence the
        /// camera's y position can be determined given a desired camera orthographic size.
        /// </summary>
        float FindCameraYPosition(float desiredOrthographicSize);

        /// <summary>
        /// The screen should never show past the back of a cruiser by more than a set
        /// amount.  Hence, given a desired camera orthographic size we can determine
        /// what camera position x values are valid.
        /// </summary>
        IPvPRange<float> FindValidCameraXPositions(float desiredOrthographicSize);

        /// <summary>
        /// Returns the camera position required for the zoomTarget's viewport position
        /// to remain the same.
        /// </summary>
        Vector3 FindZoomingCameraPosition(
            Vector2 zoomTarget,
            Vector2 targetViewportPosition,
            float newCameraOrthographicSize,
            float cameraAspectRatio,
            float cameraPositionZ);
    }
}
