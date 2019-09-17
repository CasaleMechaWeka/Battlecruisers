using BattleCruisers.Buildables.Pools;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings
{
    public enum BuildingCategory
	{
		Factory, Defence, Offence, Tactical, Ultra
	}

    // Explicitly set integner values, because the Unity inspector binds
    // to the interger values.  So now, if I decide to modify the enum
    // I don't need to adjust every single prefab that uses this enum.  
    // Thanks Manya!
    public enum BuildingFunction
    {
        Generic = 0,
        AntiAir = 1,
        AntiShip = 2,
        Shield = 3
    }

    public interface IBuilding : IBuildable, IPoolable<BuildingActivationArgs>
    {
        BuildingCategory Category { get; }
        SlotSpecification SlotSpecification { get; }
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
	}
}
