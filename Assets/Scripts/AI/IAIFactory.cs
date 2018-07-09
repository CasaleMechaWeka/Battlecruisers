namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        IArtificialIntelligence CreateBasicAI(ILevelInfo levelInfo);
        IArtificialIntelligence CreateAdaptiveAI(ILevelInfo levelInfo);
	}
}
