using System.Collections;

namespace BattleCruisers.Utils.Threading
{
    public interface ICoroutineStarter
    {
        void StartRoutine(IEnumerator routine);
    }
}