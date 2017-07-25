namespace BattleCruisers.AI.Tasks.States
{
    public class InitialState : BaseState
    {
        public InitialState(IInternalTask task)
            : base(task)
        {
        }

        public override IState Start()
        {
            _task.Start();
            return new InProgressState(_task);
        }

        public override IState Stop()
        {
            return this;
        }
    }
}