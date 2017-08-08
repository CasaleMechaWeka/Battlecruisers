using BattleCruisers.Data.Static;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers
{
    public class AntiNavalBuildOrderProvivder : AntiUnitBuildOrderProvider
    {
        public AntiNavalBuildOrderProvivder(IStaticData staticData) 
            : base(staticData, basicDefenceKey: StaticPrefabKeys.Buildings.AntiShipTurret, advancedDefenceKey: StaticPrefabKeys.Buildings.Mortar)
        {
        }

        protected override int FindNumOfSlotsToUse(int numOfDeckSlots)
        {
            return Helper.Half(numOfDeckSlots, roundUp: false);
        }
    }
}