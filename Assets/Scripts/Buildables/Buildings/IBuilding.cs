using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;

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
            ISlot parentSlot);
	}
}
