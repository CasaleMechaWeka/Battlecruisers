using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public enum PvPBuildingCategory
    {
        Factory, Defence, Offence, Tactical, Ultra
    }

    // Explicitly set integer values, because the Unity inspector binds
    // to the integer values.  So now, if I decide to modify the enum
    // I don't need to adjust every single prefab that uses this enum.  
    // Thanks Manya!
    public enum PvPBuildingFunction
    {
        Generic = 0,
        AntiAir = 1,
        AntiShip = 2,
        Shield = 3
    }

    public interface IPvPBuilding : IPvPBuildable, IPvPPoolable<PvPBuildingActivationArgs>
    {
        PvPBuildingCategory Category { get; }
        IPvPSlotSpecification SlotSpecification { get; }
        Vector3 PuzzleRootPoint { get; }

        /// <summary>
        /// True if the buliding is boostable, false otherwise.  For example, these buildings are boostable:
        /// + Air/Naval factory => Build speed
        /// + Turrets           => Fire rate
        /// + Shield            => Recharge rate
        /// + LocalBooster      => Boost multiplier
        /// 
        /// Everything else is not, such as:
        /// + Kamikaze signal
        /// + Stealth generator
        /// + Nuke launcher
        /// </summary>
        bool IsBoostable { get; }

        void Activate(PvPBuildingActivationArgs pvPBuildingActivationArgs);
    }
}
