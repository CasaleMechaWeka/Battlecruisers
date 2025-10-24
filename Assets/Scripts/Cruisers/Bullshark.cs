using BattleCruisers.Buildables;
using BattleCruisers.Data;
using static BattleCruisers.Buildables.Boost.GlobalProviders.BoostType;

namespace BattleCruisers.Cruisers
{
    public class Bullshark : Cruiser
    {
        public override void Initialise(CruiserArgs args)
        {
            if (args.Faction == Faction.Reds && ApplicationModel.SelectedLevel == 2) //Level #2 "Jimmo" would be too hard otherwise
                foreach (BoostStats boost in Boosts)
                    if (boost.boostType == BuildRateShield)
                        boost.boostAmount = 1f;
        }
    }
}