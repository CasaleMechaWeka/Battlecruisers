using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Rocket Bonus
    /// </summary>
    public class PvPPistol : PvPCruiser
    {
        public float fireRateRocketBonus;
        public float damageRocketBonus;
        public float buildSpeedForRocketBuildings;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(fireRateRocketBonus > 0);
            Assert.IsTrue(damageRocketBonus > 0);
            Assert.IsTrue(buildSpeedForRocketBuildings > 0);

            IPvPBoostProvider rocketFireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(fireRateRocketBonus);
            //CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.RocketTypeBulletBuildings.Add(rocketFireRateBoostProvider);
            
            IPvPBoostProvider rocketDamageBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(damageRocketBonus);
            //CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.RocketTypeBulletBuildings.Add(rocketDamageBoostProvider);

            IPvPBoostProvider buildSpeedForRocketProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildSpeedForRocketBuildings);
            //CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.RocketTypeBulletBuildings.Add(buildSpeedForRocketProvider);
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}
