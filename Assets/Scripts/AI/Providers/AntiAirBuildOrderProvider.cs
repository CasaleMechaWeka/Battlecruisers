using BattleCruisers.Data.Static;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers
{
    public class AntiAirBuildOrderProvivder : AntiUnitBuildOrderProvider
    {
        public AntiAirBuildOrderProvivder(IStaticData staticData) 
            : base(staticData, basicDefenceKey: StaticPrefabKeys.Buildings.AntiAirTurret, advancedDefenceKey: StaticPrefabKeys.Buildings.SamSite)
        {
        }

        protected override int FindNumOfSlotsToUse(int numOfDeckSlots)
        {
            return Helper.Half(numOfDeckSlots, roundUp: true);
        }
    }
}