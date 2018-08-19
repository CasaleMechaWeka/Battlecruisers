using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class UnitSection : MonoBehaviour
    {
        private IList<UnitsRowWrapper> _unitRows;

        public void Initialise(ItemsRowArgs<IUnit> args, IItemStateManager itemStateManager)
        {
            Helper.AssertIsNotNull(args, itemStateManager);

            _unitRows = GetComponentsInChildren<UnitsRowWrapper>().ToList();

            foreach (UnitsRowWrapper unitRow in _unitRows)
            {
                unitRow.Initialise(args);
                itemStateManager.AddItem(unitRow.UnitsRow, ItemType.Unit);
            }
        }

        // FELIX  Extend IPresentable once it does not take an activationParameter?
        public void OnPresented()
        {
            foreach (UnitsRowWrapper buildingRow in _unitRows)
            {
                buildingRow.UnitsRow.RefreshLockedStatus();
            }
        }
    }
}