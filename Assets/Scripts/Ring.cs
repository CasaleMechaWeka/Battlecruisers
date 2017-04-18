using UnityEngine;
using System.Collections;

namespace BattleCruisers
{
	public class Ring : MonoBehaviour
	{
		public int numOfPoints;
		public float radiusInM;
		public LineRenderer lineRenderer;

		public bool Enabled
		{
			set { lineRenderer.enabled = value; }
		}

		void Start()
		{
			lineRenderer.SetVertexCount(numOfPoints);
			CreatePoints();
		}

		private void CreatePoints()
		{
			float angle = 0;

			for (int i = 0; i < (numOfPoints); ++i)
			{
				float x = Mathf.Sin(Mathf.Deg2Rad * angle);
				float y = Mathf.Cos(Mathf.Deg2Rad * angle);

				lineRenderer.SetPosition(i, new Vector3(x, y, 0) * radiusInM);

				angle += (360f / numOfPoints);
			}
		}
	}
}