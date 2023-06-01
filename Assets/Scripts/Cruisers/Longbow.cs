using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using BattleCruisers.Data.Models;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases air factory build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class Longbow : Cruiser
    
    {
        public float airFactoryBuildRateBoost;
        [FormerlySerializedAs("aircarftBuildRateBoost")] public float aircraftBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 38) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                airFactoryBuildRateBoost = SetUltraCruiserUtility(args, airFactoryBuildRateBoost); 
                aircraftBuildRateBoost = SetUltraCruiserUtility(args, aircraftBuildRateBoost);
            }

            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(factoryBoostProvider);

            IBoostProvider aircraftBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);
        }
    }
}