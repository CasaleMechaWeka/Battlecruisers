using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public interface IPvPPrefabFactory
    {
        IPvPBuildableWrapper<IPvPBuilding> GetBuildingWrapperPrefab(IPvPPrefabKey buildingKey);
        PvPBuildableOutlineController CreateOutline(PvPBuildableOutlineController outlinePrefab);
        IPvPBuilding CreateBuilding(IPvPBuildableWrapper<IPvPBuilding> buildingWrapperPrefab, IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider, ulong clientID);

        IPvPBuildableWrapper<IPvPUnit> GetUnitWrapperPrefab(IPvPPrefabKey unitKey);
        PvPBuildableOutlineController GetOutline(IPvPPrefabKey unitKey);
        Task<IPvPUnit> CreateUnit(IPvPBuildableWrapper<IPvPUnit> unitWrapperPrefab, /* IPvPUIManager uiManager,*/ IPvPFactoryProvider factoryProvider);

        PvPCruiser GetCruiserPrefab(IPvPPrefabKey hullKey);
        PvPCruiser CreateCruiser(PvPCruiser cruiserPrefab, ulong ClientNetworkId, float x);
        PvPPrefab GetPrefab(string prefabPath);

        Task<IPvPExplosion> CreateExplosion(PvPExplosionKey explosionKey);
        Task<IPvPShipDeath> CreateShipDeath(PvPShipDeathKey shipDeathKey);

        Task<TPvPProjectile> CreateProjectile<TPvPProjectile, TPvPActiavtionArgs, TPvPStats>(PvPProjectileKey prefabKey, IPvPFactoryProvider factoryProvider)
            where TPvPProjectile : PvPProjectileControllerBase<TPvPActiavtionArgs, TPvPStats>
            where TPvPActiavtionArgs : PvPProjectileActivationArgs<TPvPStats>
            where TPvPStats : IPvPProjectileStats;

        Task<IPvPDroneController> CreateDrone();
        Task<IPvPAudioSourcePoolable> CreateAudioSource(IPvPDeferrer realTimeDeferrer);
    }
}
