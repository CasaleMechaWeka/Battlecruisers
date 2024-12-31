using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public enum PvPFaction
    {
        Blues, Reds
    }

    /// <summary>
    /// Used for prioritising targets, so do NOT change order!
    /// </summary>
    public enum PvPTargetValue
    {
        Low, Medium, High
    }

    public interface IPvPTarget : IPvPDamagable, IPvPRepairable, IPvPHighlightable
    {
        PvPFaction Faction { get; }
        TargetType TargetType { get; }
        Vector2 Velocity { get; }
        ReadOnlyCollection<TargetType> AttackCapabilities { get; }
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

        Action clickedRepairButton { get; set; }
    }
}
