using UnityEngine;

namespace BattleCruisers
{
    // FELIX  Unused?  Remove :)
    public class Ring
	{
		private readonly float _radiusInM;
		private readonly int _numOfPoints;
		private readonly LineRenderer _lineRenderer;

		public bool Enabled
		{
			set { _lineRenderer.enabled = value; }
		}

		public Ring(float radiusInM, int numOfPoints, LineRenderer lineRenderer)
		{
			_radiusInM = radiusInM;
			_numOfPoints = numOfPoints;
			_lineRenderer = lineRenderer;
			_lineRenderer.positionCount = _numOfPoints;

			CreatePoints();
		}

		private void CreatePoints()
		{
			float angle = 0;

			for (int i = 0; i < (_numOfPoints); ++i)
			{
				float x = Mathf.Sin(Mathf.Deg2Rad * angle);
				float y = Mathf.Cos(Mathf.Deg2Rad * angle);

				_lineRenderer.SetPosition(i, new Vector3(x, y, 0) * _radiusInM);

				angle += (360f / (_numOfPoints - 1));
			}
		}
	}
}