using System.Collections.ObjectModel;

namespace BattleCruisers.Utils.DataStrctures
{
    public interface ICircularList<T>
	{
		T Next();
		ReadOnlyCollection<T> Items { get; }
	}
}
