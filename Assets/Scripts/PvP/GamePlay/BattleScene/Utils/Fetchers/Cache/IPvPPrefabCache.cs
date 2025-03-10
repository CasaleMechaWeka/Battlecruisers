using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public interface IPvPPrefabCache
    {
        // Single prefab caches
        PvPDroneController Drone { get; }
        PvPAudioSourceInitialiser AudioSource { get; }

        // Multiple prefab caches
        PvPBuildableWrapper<IPvPBuilding> GetBuilding(IPrefabKey key);
        PvPBuildableWrapper<IPvPUnit> GetUnit(IPrefabKey key);
        PvPCruiser GetCruiser(IPrefabKey key);
        PvPExplosionController GetExplosion(IPrefabKey key);
        PvPShipDeathInitialiser GetShipDeath(IPrefabKey key);
        PvPPrefab GetPrefab(string prefabPath);
        PvPBuildableOutlineController GetOutline(IPrefabKey key);

        // Multiple untyped prefab caches
        TProjectile GetProjectile<TProjectile>(IPrefabKey prefabKey) where TProjectile : PvPProjectile;
    }
}