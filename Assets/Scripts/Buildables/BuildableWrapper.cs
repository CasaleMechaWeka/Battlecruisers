using BattleCruisers.Fetchers;
using UnityEngine;

namespace BattleCruisers.Buildables
{
	public abstract class BuildableWrapper : MonoBehaviour, IAwakable 
	{ 
		public abstract void Awake();

		// Awake() is synonymous to the prefabs constructor.  When the prefab is instantiated in the scene,
		// Awake is called.  Because the original copy of all prefabs will never be instantiated (only copies of it
		// made, and those copies will be instantiated), need to explicitly call Awake().
		public void FakeAwake()
		{
			IAwakable[] awakables = GetComponentsInChildren<IAwakable>();
			foreach (IAwakable awakable in awakables)
			{
				awakable.Awake();
			}
		}
	}
}

