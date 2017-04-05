using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers
{
	public class BuildableProgressController : MonoBehaviour 
	{
		public Image image;

		// Use this for initialization
		void Start () 
		{
			image.fillAmount = 0.8f;
		}
	}
}
