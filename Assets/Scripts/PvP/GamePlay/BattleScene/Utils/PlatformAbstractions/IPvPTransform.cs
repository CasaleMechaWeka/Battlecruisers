using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public interface IPvPTransform
    {
        Transform PlatformObject { get; }

        Vector3 Position { get; set; }
        Vector3 EulerAngles { get; }
        Vector3 Right { get; }
        Vector3 Up { get; }
        Quaternion Rotation { get; }
        bool IsMirroredAcrossYAxis { get; }

        void Rotate(Vector3 rotationChangeVector);
        void SetParent(Transform parent, bool worldPositionStays);
    }
}
