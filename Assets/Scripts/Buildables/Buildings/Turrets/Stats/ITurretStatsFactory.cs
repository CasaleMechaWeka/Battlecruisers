using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface ITurretStatsFactory
    {
        // FELIX  Unused?
        //IBasicTurretStats CreateBoostedBasicTurretStats(IBasicTurretStats baseTurretStats);

        ITurretStats CreateBoostedTurretStats(ITurretStats baseTurretStats);
    }
}
