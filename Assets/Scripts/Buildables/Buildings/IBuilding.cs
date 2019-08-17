using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils.Factories;
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

    public interface IBuilding : IBuildable
    {
        BuildingCategory Category { get; }
        SlotSpecification SlotSpecification { get; }
        Vector3 PuzzleRootPoint { get; }

        void Initialise(
            ICruiser parentCruiser, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IFactoryProvider factoryProvider,
            ICruiserSpecificFactories cruiserSpecificFactories,
            ISlot parentSlot,
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler);
	}
}
