using System;

namespace BattleCruisers.Utils.Threading
{
    // FELIX  Remove :)
    public interface IDeferrer
    {
        void Defer(Action action);
	}
}
