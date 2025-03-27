namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        IArtificialIntelligence CreateBasicAI(LevelInfo levelInfo);
        IArtificialIntelligence CreateAdaptiveAI(LevelInfo levelInfo);
    }
}
