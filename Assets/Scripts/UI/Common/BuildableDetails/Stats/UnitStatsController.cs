using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public class UnitStatsController : BuildableStatsController<IUnit>
	{
        // For units that can attack both ships and the cruiser (ships),
        // just show their ship damage.
        protected override float GetAntiCruiserDamage(IUnit item)
        {
            if (item.AttackCapabilities.Contains(TargetType.Ships))
            {
                // Eg:  Ships (Attack boat, frigate, etc)
                return 0;
            }
            else
            {
                // Eg:  Bomber
                return base.GetAntiCruiserDamage(item);
            }
        }
	}
}