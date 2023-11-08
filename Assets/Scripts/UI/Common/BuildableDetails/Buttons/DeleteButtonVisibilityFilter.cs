using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class DeleteButtonVisibilityFilter : IFilter<ITarget>
    {
        // Player building
        public bool IsMatch(ITarget target)
        {
            return
                target != null
                && target.Faction == Faction.Blues
                && target.TargetType == TargetType.Buildings
                && target.IsInScene;
        }

        public bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return
                target != null
                && target.Faction == Faction.Blues
                && target.TargetType == TargetType.Buildings
                && target.IsInScene;
        }
    }
}
