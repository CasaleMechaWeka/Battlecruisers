using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class CircularList<T> : ICircularList<T>
	{
		private IList<T> _items;
		private int _index;

		public int Index
		{
			get => _index;
			set
			{
				if (value < 0
					|| value >= _items.Count)
				{
					throw new ArgumentException($"Invalid index {value}.  Must be >= 0 and < {_items.Count}");
				}

				_index = value;
			}
		}

		public ReadOnlyCollection<T> Items { get; }

		public CircularList(T[] items) : this(new List<T>(items)) { }
        
        public CircularList(IList<T> items)
        {
            Assert.IsTrue(items.Count != 0);
			
			_items = items;
			Items = new ReadOnlyCollection<T>(_items);
            _index = _items.Count - 1;
		}

		public T Next()
		{
            _index = (_index + 1) % _items.Count;
			return _items[_index];
		}

		public T Current()
		{
			return _items[_index];
		}
	}
}
