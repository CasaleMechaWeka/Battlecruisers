using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public class PvPCameraTarget : IPvPCameraTarget
    {
        public Vector3 Position { get; }
        public float OrthographicSize { get; }

        public PvPCameraTarget(Vector3 position, float orthographicSize)
        {
            Position = position;
            OrthographicSize = orthographicSize;
        }

        public override bool Equals(object obj)
        {
            PvPCameraTarget other = obj as PvPCameraTarget;
            return
                other != null
                && Position == other.Position
                && Mathf.Approximately(OrthographicSize, other.OrthographicSize);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Position, OrthographicSize);
        }

        public override string ToString()
        {
            return "CameraTarget: " + Position + "  Orthographic size: " + OrthographicSize;
        }
    }
}
