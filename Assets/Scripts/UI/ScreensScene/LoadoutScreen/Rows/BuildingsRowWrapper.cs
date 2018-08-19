using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class BuildingsRowWrapper : BuildablesRowWrapper<IBuilding, BuildingKey>
    {
        public BuildingCategory category;

        public override void Initialise(IItemsRowArgs<IBuilding> args)
        {
            Assert.IsNotNull(args);

            LoadoutBuildingItemsRow loadoutRow = GetComponentInChildren<LoadoutBuildingItemsRow>();
            Assert.IsNotNull(loadoutRow);
            loadoutRow.Initialise(args, category);

            BuildablesRow = loadoutRow;
        }
    }
}
