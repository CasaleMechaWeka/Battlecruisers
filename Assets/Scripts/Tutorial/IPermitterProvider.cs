using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders;

namespace BattleCruisers.Tutorial
{
    public interface IPermitterProvider
    {
		ISlotPermitter SlotPermitter { get; }
		IBuildingCategoryPermitter BuildingCategoryPermitter { get; }

        IActivenessDecider<IBuildable> BuildingActivenessDecider { get; }
        IBuildingPermitter BuildingPermitter { get; }

        BasicDecider NavigationPermitter { get; }
    }
}
