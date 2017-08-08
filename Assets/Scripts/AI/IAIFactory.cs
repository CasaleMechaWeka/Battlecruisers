using BattleCruisers.Cruisers;
using BattleCruisers.Data;

namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        void CreateBasicAI(ILevel level, ISlotWrapper slotWrapper);
        void CreateAdaptiveAI(ILevel level, ISlotWrapper slotWrapper);
	}
}