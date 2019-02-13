using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IItemDetailsManager
    {
        IBroadcastingProperty<ITarget> SelectedItem { get; }

        void ShowDetails(IBuilding building);
        void ShowDetails(IUnit unit);
        void ShowDetails(ICruiser cruiser);
        void HideDetails();
    }
}
