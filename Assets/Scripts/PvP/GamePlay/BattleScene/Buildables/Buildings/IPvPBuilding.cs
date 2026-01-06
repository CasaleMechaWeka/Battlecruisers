using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    // Explicitly set integer values, because the Unity inspector binds
    // to the integer values.  So now, if I decide to modify the enum
    // I don't need to adjust every single prefab that uses this enum.  
    // Thanks Manya!

    public interface IPvPBuilding : IPvPBuildable, IPoolable<PvPBuildingActivationArgs>
    {
        BuildingCategory Category { get; }
        ISlotSpecification SlotSpecification { get; }
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
        int variantIndex { get; set; }
        void ApplyVariantStats(StatVariant statVariant);
        new void Activate(PvPBuildingActivationArgs pvPBuildingActivationArgs);
    }
}
