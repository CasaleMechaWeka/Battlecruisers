using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    // FELIX  Also create UnitSection :)
    // FELIX  Avoid duplciate code with UnitSection
    public class BuildingSection : MonoBehaviour
    {
        private IList<BuildingsRowWrapper> _buildingRows;

        public void Initialise(ItemsRowArgs<IBuilding> args, IItemStateManager itemStateManager)
        {
            Helper.AssertIsNotNull(args, itemStateManager);

            _buildingRows = GetComponentsInChildren<BuildingsRowWrapper>().ToList();

            foreach (BuildingsRowWrapper buildingRow in _buildingRows)
            {
                buildingRow.Initialise(args);
                itemStateManager.AddItem(buildingRow.BuildingsRow, ItemType.Building);
            }
        }

        // FELIX  Extend IPresentable once it does not take an activationParameter?
        public void OnPresented()
        {
            foreach (BuildingsRowWrapper buildingRow in _buildingRows)
            {
                buildingRow.BuildingsRow.RefreshLockedStatus();
            }
        }
    }
}