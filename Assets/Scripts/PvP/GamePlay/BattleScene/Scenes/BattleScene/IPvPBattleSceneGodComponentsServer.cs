using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneGodComponentsServer
    {
        IPvPDeferrer Deferrer { get; }
        IPvPDeferrer RealTimeDeferrer { get; }

        IPvPUpdaterProvider UpdaterProvider { get; }

        IPvPLifetimeEventBroadcaster LifetimeEvents { get; }

    }
}