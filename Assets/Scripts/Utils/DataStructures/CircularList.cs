using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
	public class CircularList<T> : ICircularList<T>
	{
		private T[] _items;
		private int _index;

		public CircularList(T[] items)
		{
			Assert.IsTrue(items.Length != 0);

			_items = items;
			_index = _items.Length -1;
		}

		public T Next()
		{
			_index = (_index + 1) % _items.Length;
			return _items[_index];
		}
	}
}
