using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitPoolProvider : IUnitPoolProvider
    {
        private readonly IUIManager _uiManager;
        private readonly IFactoryProvider _factoryProvider;

        // Aircraft
        public IPool<Unit, BuildableActivationArgs> BomberPool { get; }
        public IPool<Unit, BuildableActivationArgs> FighterPool { get; }
        public IPool<Unit, BuildableActivationArgs> GunshipPool { get; }
        public IPool<Unit, BuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        public IPool<Unit, BuildableActivationArgs> AttackBoatPool { get; }
        public IPool<Unit, BuildableActivationArgs> FrigatePool { get; }
        public IPool<Unit, BuildableActivationArgs> DestroyerPool { get; }
        public IPool<Unit, BuildableActivationArgs> ArchonPool { get; }

        public UnitPoolProvider(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            Helper.AssertIsNotNull(uiManager, factoryProvider);

            _uiManager = uiManager;
            _factoryProvider = factoryProvider;

            // Aircraft
            BomberPool = CreatePool(StaticPrefabKeys.Units.Bomber);
            FighterPool = CreatePool(StaticPrefabKeys.Units.Fighter);
            GunshipPool = CreatePool(StaticPrefabKeys.Units.Gunship);
            TestAircraftPool = CreatePool(StaticPrefabKeys.Units.TestAircraft);

            // Ship
            AttackBoatPool = CreatePool(StaticPrefabKeys.Units.AttackBoat);
            FrigatePool = CreatePool(StaticPrefabKeys.Units.Frigate);
            DestroyerPool = CreatePool(StaticPrefabKeys.Units.Destroyer);
            ArchonPool = CreatePool(StaticPrefabKeys.Units.ArchonBattleship);
        }

        private IPool<Unit, BuildableActivationArgs> CreatePool(IPrefabKey unitKey)
        {
            return
                new Pool<Unit, BuildableActivationArgs>(
                    new UnitFactory(
                        _factoryProvider.PrefabFactory,
                        unitKey,
                        _uiManager,
                        _factoryProvider));
        }
    }
}