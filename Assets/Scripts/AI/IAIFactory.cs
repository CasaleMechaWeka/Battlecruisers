using BattleCruisers.Data;

namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        void CreateBasicAI(ILevel level);
        void CreateAdaptiveAI(ILevel level, int numOfPlatformSlots);
	}
}