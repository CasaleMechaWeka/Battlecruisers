namespace BattleCruisers.AI
{
    public interface ITaskConsumer
    {
        // FELIX  AI stats?
        // - build speed bonus?
        // - delay between tasks?

        void ConsumeTask(ITask task);
    }
}
