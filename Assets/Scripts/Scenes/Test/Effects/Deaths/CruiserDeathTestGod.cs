using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils.Factories;
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathTestGod : MonoBehaviour
    {
        void Start()
        {
            CreateAndDestroyCruiser();
        }

        private void CreateAndDestroyCruiser()
        {
            Cruiser cruiser = FindObjectOfType<Cruiser>();
            Assert.IsNotNull(cruiser);

            ICruiserSpecificFactories cruiserSpecificFactories = Substitute.For<ICruiserSpecificFactories>();
            GlobalBoostProviders globalBoostProviders = new GlobalBoostProviders();
            cruiserSpecificFactories.GlobalBoostProviders.Returns(globalBoostProviders);

            ICruiserArgs cruiserArgs
                = new CruiserArgs(
                    Buildables.Faction.Reds,
                    enemyCruiser: Substitute.For<ICruiser>(),
                    uiManager: Substitute.For<IUIManager>(),
                    droneManager: Substitute.For<IDroneManager>(),
                    droneFocuser: Substitute.For<IDroneFocuser>(),
                    droneConsumerProvider: Substitute.For<IDroneConsumerProvider>(),
                    factoryProvider: Substitute.For<IFactoryProvider>(),
                    cruiserSpecificFactories: cruiserSpecificFactories,
                    facingDirection: Direction.Right,
                    repairManager: Substitute.For<IRepairManager>(),
                    fogStrength: BattleCruisers.Cruisers.Fog.FogStrength.Weak,
                    helper: Substitute.For<ICruiserHelper>(),
                    highlightableFilter: Substitute.For<ISlotFilter>(),
                    buildProgressCalculator: Substitute.For<IBuildProgressCalculator>(),
                    buildingDoubleClickHandler: Substitute.For<IDoubleClickHandler<IBuilding>>(),
                    cruiserDoubleClickHandler: Substitute.For<IDoubleClickHandler<ICruiser>>(),
                    fogOfWarManager: Substitute.For<BCUtils.IManagedDisposable>());

            cruiser.StaticInitialise();
            cruiser.Initialise(cruiserArgs);
            DestroyCruiser(cruiser);
        }

        protected virtual void DestroyCruiser(Cruiser cruiser)
        {
            cruiser.Destroy();
        }
    }
}