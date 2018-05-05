using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons;

namespace BattleCruisers.Tutorial
{
    public interface IPermitterProvider
    {
        ISlotPermitter SlotPermitter { get; }
        IBuildingPermitter BuildingPermitter { get; }
        IBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        BasicDecider NavigationPermitter { get; }
    }
}
