namespace BattleCruisers.AI.Tasks
{
    public class DelayProvider : IDelayProvider
    {
        public float DelayInS { get; set; }

        public DelayProvider(float initialDelayInS)
        {
            DelayInS = initialDelayInS;
        }
    }
}