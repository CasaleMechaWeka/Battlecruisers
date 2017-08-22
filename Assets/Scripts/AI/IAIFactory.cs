namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        void CreateBasicAI(ILevelInfo levelInfo);
        void CreateAdaptiveAI(ILevelInfo levelInfo);
	}
}
