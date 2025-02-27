using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.Tasks.States;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    // Converts:
    /// <summary>
    /// Converts:
    /// + ITask.Start()
    /// + ITask.Stop()
    /// 
    /// Into:
    /// + IInternalTask.Start() => Only ever called once
    /// + IInternalTask.Stop()  => Only called if started or resumed
    /// + IInternalTask.Resume()=> Only called if stopped
    /// </summary>
    public class PvPPrioritisedTask : IPrioritisedTask, ICompletedEventEmitter
    {
        private readonly ITask _task;
        private IState _currentState;

        public TaskPriority Priority { get; }

        public event EventHandler<EventArgs> Completed;

        public PvPPrioritisedTask(TaskPriority priority, ITask task)
        {
            Priority = priority;
            _task = task;
            _currentState = new PvPInitialState(_task, this);

            _task.Completed += _task_Completed;
        }

        public void Start()
        {
            _currentState = _currentState.Start();
        }

        public void Stop()
        {
            _currentState = _currentState.Stop();
        }

        public void EmitCompletedEvent()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        private void _task_Completed(object sender, EventArgs e)
        {
            _task.Completed -= _task_Completed;
            _currentState = _currentState.OnCompleted();
        }

        public override string ToString()
        {
            return _task.ToString();
        }
    }
}
