using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Rocket Bonus
    /// </summary>
    public class Pistol : Cruiser
    {
        public float fireRateRocketBonus;
        public float damageRocketBonus;
        public float buildSpeedForRocketBuildings;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 85) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                fireRateRocketBonus = SetUltraCruiserUtility(args, fireRateRocketBonus);
                damageRocketBonus = SetUltraCruiserUtility(args, damageRocketBonus);
                buildSpeedForRocketBuildings = SetUltraCruiserUtility(args, buildSpeedForRocketBuildings);
            }
            base.Initialise(args);

            Assert.IsTrue(fireRateRocketBonus > 0);
            Assert.IsTrue(damageRocketBonus > 0);
            Assert.IsTrue(buildSpeedForRocketBuildings > 0);

            IBoostProvider rocketFireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(fireRateRocketBonus);
            //CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(rocketFireRateBoostProvider);
            
            IBoostProvider rocketDamageBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(damageRocketBonus);
            //CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(rocketDamageBoostProvider);

            IBoostProvider buildSpeedForRocketProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildSpeedForRocketBuildings);
            //CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(buildSpeedForRocketProvider);
        }
    }
}
