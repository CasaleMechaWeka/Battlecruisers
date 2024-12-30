namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public interface IPvPState
    {
        IPvPState Start();
        IPvPState Stop();
        IPvPState OnCompleted();
    }
}