using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Factories;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathTestGod : MonoBehaviour
    {
        void Start()
        {
            Cruiser cruiser = FindObjectOfType<Cruiser>();
            Assert.IsNotNull(cruiser);

            //ICruiserArgs cruiserArgs
            //    = new CruiserArgs(
            //        Buildables.Faction.Reds,
            //        enemyCruiser: Substitute.For<ICruiser>(),
            //        uiManager: Substitute.For<IUIManager>(),
            //        droneManager: Substitute.For<IDroneManager>(),
            //        droneFocuser: Substitute.For<IDroneFocuser>(),
            //        droneConsumerProvider: Substitute.For<IDroneConsumerProvider>(),
            //        factoryProvider: Substitute.For<IFactoryProvider>(),
            //        facingDirection: Direction.Right,
            //        // FELIX  NEXT  Doesn't use interface, uses RepairManager concrete type :/
            //        repairManager: Substitute.For<IRepairManager>(),
            //        fogStrength: BattleCruisers.Cruisers.Fog.FogStrength.Weak,
            //        helper: Substitute.For<ICruiserHelper>(),
            //        highlightableFilter: Substitute.For<ISlotFilter>(),
            //        buildProgressCalculator: Substitute.For<IBuildProgressCalculator>(),
            //        buildingDoubleClickHandler,
            //        cruiserDoubleClickHandler,
            //        fogOfWarManager);
        }
    }
}