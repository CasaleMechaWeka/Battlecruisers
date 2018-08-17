using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class DeleteButtonVisibilityFilter : IFilter<ITarget>
    {
        // Player bulidnig
        public bool IsMatch(ITarget target)
        {
            return
                target != null
                && target.Faction == Faction.Blues
                && target.TargetType == TargetType.Buildings;
        }
    }
}
