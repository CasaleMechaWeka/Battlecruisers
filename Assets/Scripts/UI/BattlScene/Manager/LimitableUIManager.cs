using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Manager
{
    // FELIX  Test :D
    /// <summary>
    /// Wraps a normal UIManager, and only allows the noraml UIManager methods
    /// to execute if the IUIManagerPermissions allow it.
    /// </summary>
    public class LimitableUIManager : UIManager
    {
        private readonly IUIManagerPermissions _permissions;

        public LimitableUIManager(IManagerArgs args, IUIManagerPermissions permissions)
            : base(args)
        {
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

        public override void SelectBuilding(IBuilding building, ICruiser buildingParent)
        {
            if (_permissions.CanShowItemDetails)
            {
                base.SelectBuilding(building, buildingParent);
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
