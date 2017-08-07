using BattleCruisers.Data;

namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        void CreateBasicAI(ILevel level, int numOfPlatformSlots);
        void CreateAdaptiveAI(ILevel level, int numOfPlatformSlots);
	}
}