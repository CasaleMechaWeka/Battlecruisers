namespace BattleCruisers.AI
{
    public interface IAIManager
    {
        IArtificialIntelligence CreateAI(ILevelInfo levelInfo);
	}
}
