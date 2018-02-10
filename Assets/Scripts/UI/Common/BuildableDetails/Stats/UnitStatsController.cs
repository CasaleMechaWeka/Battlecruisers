using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public class UnitStatsController : BaseBuildableStatsController<IUnit>
	{
        // Although some units (eg: ships) can attack both other ships and cruiser, do not
        // want to show this damage twice.  Hence, do not show anti cruiser damage.
        protected override float GetAntiCruiserDamage(IUnit item)
        {
            return 0;
        }
	}
}