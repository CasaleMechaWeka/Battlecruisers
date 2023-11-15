using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public interface IPvPUnitPoolProvider
    {
        // Aircraft
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> BomberPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> FighterPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> GunshipPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> SteamCopterPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> BroadswordPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> AttackBoatPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> AttackRIBPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> FrigatePool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> DestroyerPool { get; }
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> ArchonPool { get; }
    }
}