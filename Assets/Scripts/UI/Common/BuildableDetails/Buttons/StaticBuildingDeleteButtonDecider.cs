using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class StaticBuildingDeleteButtonDecider : StaticDecider<IBuilding>
    {
        public StaticBuildingDeleteButtonDecider(bool shouldBeEnabled) 
            : base(shouldBeEnabled)
        {
        }
    }
}
