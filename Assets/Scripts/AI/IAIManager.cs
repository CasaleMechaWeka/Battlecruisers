using BattleCruisers.Cruisers;
using BattleCruisers.Data;

namespace BattleCruisers.AI
{
    public interface IAIManager
    {
        void CreateAI(ILevel currentLevel, ICruiserController playerCruiser, ICruiserController aiCruiser);
	}
}