using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public interface IPvPState
    {
        Task<IPvPState> Start();
        IPvPState Stop();
        IPvPState OnCompleted();
    }
}