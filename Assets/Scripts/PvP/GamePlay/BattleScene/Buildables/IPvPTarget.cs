using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public enum PvPFaction
    {
        Blues, Reds
    }

    public enum PvPTargetType
    {
        Aircraft, Ships, Cruiser, Buildings, Rocket, Satellite, PaddleMine
    }

    /// <summary>
    /// Used for prioritising targets, so do NOT change order!
    /// </summary>
    public enum PvPTargetValue
    {
        Low, Medium, High
    }

    public interface IPvPTarget : IPvPDamagable, IRepairable, IHighlightable
    {
        PvPFaction Faction { get; }
        PvPTargetType TargetType { get; }
        Vector2 Velocity { get; }
        ReadOnlyCollection<PvPTargetType> AttackCapabilities { get; }
        PvPTargetValue TargetValue { get; }
        Color Color { set; }
        bool IsInScene { get; }
        Vector2 Size { get; }
        IPvPTransform Transform { get; }

        Vector2 Position { get; set; }
        Quaternion Rotation { get; set; }

        bool IsShield();
        void SetBuildingImmunity(bool boo);
        bool IsBuildingImmune();
    }
}
