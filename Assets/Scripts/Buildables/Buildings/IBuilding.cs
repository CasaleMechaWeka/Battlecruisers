using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers;
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

        void Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, IUIManager uiManager, IFactoryProvider factoryProvider, IBoostProviderList localBoostProviders);
	}
}