using BattleCruisers.Fetchers;
using UnityEngine;

namespace BattleCruisers.Buildables
{
	public abstract class BuildableWrapper : MonoBehaviour, IAwakable 
	{ 
		public abstract void Awake();
	}
}

