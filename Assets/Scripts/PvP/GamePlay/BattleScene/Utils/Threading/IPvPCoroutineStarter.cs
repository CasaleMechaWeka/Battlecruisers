using System.Collections;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public interface IPvPCoroutineStarter
    {
        void StartRoutine(IEnumerator routine);
    }
}