using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.Buildings
{
    public enum BuildingCategory
	{
		Factory, Defence, Offence, Tactical, Ultra
	}

    public interface IBuilding : IBuildable
    {
        BuildingCategory Category { get; }
		float CustomOffsetProportion { get; }
        bool PreferCruiserFront { get; }
        SlotType SlotType { get; }

        void Initialise(
            ICruiser parentCruiser, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IFactoryProvider factoryProvider, 
            ISlot parentSlot,
            IBuildingDoubleClickHandler buildingDoubleClickHandler);
	}
}
