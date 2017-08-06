using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class CircularList<T> : ICircularList<T>
	{
		private T[] _items;
		private int _index;

		public ReadOnlyCollection<T> Items { get; private set; }

		public CircularList(T[] items)
		{
			Assert.IsTrue(items.Length != 0);

			_items = items;
			Items = new ReadOnlyCollection<T>(_items);
			_index = _items.Length -1;
		}

		public T Next()
		{
			_index = (_index + 1) % _items.Length;
			return _items[_index];
		}
	}
}
