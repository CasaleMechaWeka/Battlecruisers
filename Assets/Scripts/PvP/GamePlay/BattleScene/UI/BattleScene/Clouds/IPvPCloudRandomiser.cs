using BattleCruisers.UI.BattleScene.Clouds;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds
{
    public interface IPvPCloudRandomiser
    {
        void RandomiseStartingPosition(ICloud leftCloud, ICloud rightCloud);
    }
}