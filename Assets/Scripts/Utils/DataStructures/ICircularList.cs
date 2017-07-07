using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
	public interface ICircularList<T>
	{
		T Next();
		ReadOnlyCollection<T> Items { get; }
	}
}
