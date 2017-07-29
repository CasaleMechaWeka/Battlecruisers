using System;

namespace BattleCruisers.Utils.Threading
{
    public interface IDeferrer
    {
        void DeferToFrameEnd(Action action);
	}
}