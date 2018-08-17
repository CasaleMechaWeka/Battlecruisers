using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ChooseTargetFilter : IFilter<ITarget>
    {
        // AI buildings or cruiser
        public bool IsMatch(ITarget target)
        {
            return
                target != null
                && target.Faction == Faction.Reds
                && (target.TargetType == TargetType.Buildings
                    || target.TargetType == TargetType.Cruiser);
        }
    }
}
