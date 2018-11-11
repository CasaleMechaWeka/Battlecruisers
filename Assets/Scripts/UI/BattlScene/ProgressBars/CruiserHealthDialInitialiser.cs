using BattleCruisers.Cruisers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class CruiserHealthDialInitialiser : HealthDialInitialiserBase<ICruiser>
    {
        protected override IFilter<ICruiser> CreateVisibilityFilter()
        {
            return new StaticFilter<ICruiser>(isMatch: true);
        }
    }
}