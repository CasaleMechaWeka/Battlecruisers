namespace BattleCruisers.AI.Tasks.States
{
    public interface IState
    {
        IState Start();
        IState Stop();
        IState OnCompleted();
    }
}