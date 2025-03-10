using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ChooseTargetButtonVisibilityFilter : IFilter<ITarget>
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
        public bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return
                target != null
                && target.Faction == Faction.Reds
                && (target.TargetType == TargetType.Buildings
                    || target.TargetType == TargetType.Cruiser);
        }
    }
}
