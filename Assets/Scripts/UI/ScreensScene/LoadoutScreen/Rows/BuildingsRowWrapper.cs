using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class BuildingsRowWrapper : MonoBehaviour
    {
        public BuildingCategory category;

        public void Initialise(IItemsRowArgs<IBuilding> args)
        {
            Assert.IsNotNull(args);

            LoadoutBuildingItemsRow loadoutRow = GetComponentInChildren<LoadoutBuildingItemsRow>();
            Assert.IsNotNull(loadoutRow);

            IItemsRow<IBuilding> buildingsRow = new BuildingItemsRow(args, loadoutRow, category);
            buildingsRow.SetupUI();
        }
    }
}
