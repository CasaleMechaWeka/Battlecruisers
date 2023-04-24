using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildable
{
    public enum Faction
    {
        Blues, Reds
    }

    public enum TargetType
    {
        Aircraft, Ships, Cruiser, Buildings, Rocket, Satellite, PaddleMine
    }

    /// <summary>
    /// Used for prioritising targets, so do NOT change order!
    /// </summary>
    public enum TargetValue
    {
        Low, Medium, High
    }

    public interface IPvPTarget : IPvPDamagable, IRepairable, IHighlightable
    {
        Faction Faction { get; }
        TargetType TargetType { get; }
        Vector2 Velocity { get; }
        ReadOnlyCollection<TargetType> AttackCapabilities { get; }
        TargetValue TargetValue { get; }
        Color Color { set; }
        bool IsInScene { get; }
        Vector2 Size { get; }
        ITransform Transform { get; }

        Vector2 Position { get; set; }
        Quaternion Rotation { get; set; }

        bool IsShield();
        void SetBuildingImmunity(bool boo);
        bool IsBuildingImmune();
    }
}
