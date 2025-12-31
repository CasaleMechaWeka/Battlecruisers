using System.Collections.ObjectModel;

namespace BattleCruisers.Utils.DataStrctures
{
    public interface ICircularList<T>
	{
		int Index { get; set; }

		T Next();
		T Current();
		ReadOnlyCollection<T> Items { get; }
	}
}
