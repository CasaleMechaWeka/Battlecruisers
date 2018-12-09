using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Manager
{
    /// <summary>
    /// Wraps a normal UIManager, and only allows the noraml UIManager methods
    /// to execute if the IUIManagerPermissions allow it.
    /// </summary>
    public class LimitableUIManagerNEW : UIManagerNEW
    {
        private IUIManagerPermissions _permissions;

        public void Initialise(ManagerArgsNEW args, IUIManagerPermissions permissions)
        {
            base.Initialise(args);

            Assert.IsNotNull(permissions);
            _permissions = permissions;
        }

        public override void HideItemDetails()
        {
            if (_permissions.CanDismissItemDetails)
            {
                base.HideItemDetails();
            }
        }

        public override void SelectBuilding(IBuilding building)
        {
            if (_permissions.CanShowItemDetails)
            {
                base.SelectBuilding(building);
            }
        }

        public override void ShowUnitDetails(IUnit unit)
        {
            if (_permissions.CanShowItemDetails)
            {
                base.ShowUnitDetails(unit);
            }
        }

        public override void ShowCruiserDetails(ICruiser cruiser)
        {
            if (_permissions.CanShowItemDetails)
            {
                base.ShowCruiserDetails(cruiser);
            }
        }
    }
}
