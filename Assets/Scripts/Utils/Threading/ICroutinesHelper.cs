using System;

namespace BattleCruisers.Utils.Threading
{
    public interface ICoroutinesHelper
    {
        void DeferToFrameEnd(Action action);
	}
}