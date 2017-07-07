using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
	public interface ICircularList<T>
	{
		T Next();
	}
}
