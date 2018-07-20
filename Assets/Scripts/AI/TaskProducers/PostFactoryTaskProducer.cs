using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.AI.TaskProducers
{
    /// <summary>
    /// Creates a WaitForUnitConstructionTask when a factory starts being constructed.
    /// 
    /// This means that after building a factory, the AI cruiser should wait for some
    /// units to be produced before constructing the next building.
    /// </summary>
    /// FELIX  Remove?  Also WaitForConstructionTask?
    public class PostFactoryTaskProducer : TaskProducerBase
    {
        public PostFactoryTaskProducer(
            ITaskList tasks, 
            ICruiserController cruiser, 
            ITaskFactory taskFactory, 
            IPrefabFactory prefabFactory) 
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            // FELIX  TEMP
            //_cruiser.StartedConstruction += _cruiser_StartedConstruction;
        }

        private void _cruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null)
            {
                Logging.Log(Tags.AI, "_cruiser_StartedConstruction(): of factory: " + factory);

                IPrioritisedTask taskToAdd = _taskFactory.CreateWaitForUnitConstructionTask(TaskPriority.Low, factory);
                _tasks.Add(taskToAdd);
            }
        }

        public override void DisposeManagedState()
        {
            _cruiser.StartedConstruction -= _cruiser_StartedConstruction;
            base.DisposeManagedState();
        }
    }
}
